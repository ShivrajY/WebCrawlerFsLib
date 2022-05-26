// Shri Ganesha //
module WebCrawlerFsLib.WebCrawler

open System
open System.Linq
open System.Net
open System.Net.Http
open System.IO
open System.Collections.Concurrent
open System.Threading
open System.Threading.Tasks
open AngleSharp
open AngleSharp.Css
open AngleSharp.Js
open AngleSharp.Io.Network
open AngleSharp.Html.Parser

type LogData =
    {
      LinksInQueue: int
      CheckedLinks: int
      Message:string
     }

let LIMIT = 100

type RequestGate(n:int) =
    let semaphore = new Semaphore(initialCount=n, maximumCount = n)
    member _.AsyncAcquire(?timeout) = 
        async{
                let! ok = Async.AwaitWaitHandle(semaphore, ?millisecondsTimeout = timeout)
                if (ok) then
                   return {
                            new System.IDisposable with 
                                member _.Dispose() = semaphore.Release()|>ignore
                           }
                else  
                    return! failwith "Couldn't aquire semaphore"    
        }

type FileMessage =
     | Save of data:(bool*string*string)
     | Flush
     | Quit
 
type FileSaverAgent(totalLinksFile:string, goodLinksFile:string, badLinksFile:string) =
    let totalLinksFileSW = File.AppendText(totalLinksFile)
    let goodLinksFileSW = File.AppendText(goodLinksFile)
    let badLinksFileSW = File.AppendText(badLinksFile)
    let agent =
            MailboxProcessor.Start(
                fun (inbox: MailboxProcessor<FileMessage>) -> 
                    let rec loop() =
                        async {
                                 let! message = inbox.Receive()                   
                                 match message with
                                 | Quit -> return()
                                 | Save (b,parent, url) -> 
                                        let line = sprintf "\"%s\",\"%s\"" parent url
                                        do! totalLinksFileSW.WriteLineAsync(line)|>Async.AwaitTask
                                        match b with
                                        | true -> do! goodLinksFileSW.WriteLineAsync(line)|>Async.AwaitTask
                                        | false -> do! badLinksFileSW.WriteLineAsync(line)|>Async.AwaitTask
                                 | Flush -> totalLinksFileSW.Flush()
                                            goodLinksFileSW.Flush()
                                            badLinksFileSW.Flush()
                                 return! loop() 
                        }
                    loop()
                )
    do 
       totalLinksFileSW.AutoFlush <- true
       goodLinksFileSW.AutoFlush <- true
       badLinksFileSW.AutoFlush <- true 
       
    interface System.IDisposable with
      member x.Dispose() = 
            totalLinksFileSW.Dispose()
            goodLinksFileSW.Dispose()
            badLinksFileSW.Dispose()
            agent.Post(FileMessage.Quit)
   
    member _.SaveToFiles(data: (bool * string * string)) = agent.Post(Save(data))
    member _.FlushFiles() = agent.Post(Flush)

ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true)
ServicePointManager.DefaultConnectionLimit <- LIMIT
ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Tls11 ||| SecurityProtocolType.Tls

type Crawler(url:Uri,allLinksFile:string, goodFile:string, badFile:string, cancellationToken:CancellationToken, log:LogData->unit) =
        let baseUrl = $"{url.Scheme}://{url.Authority}"

        let linksDictionary = new ConcurrentDictionary<string,string>()             
        let blockingCollection = new BlockingCollection<string*string>()

        let fileSaverAgent = new FileSaverAgent(allLinksFile, goodFile, badFile)

        //let webRequestGate = RequestGate(5)

        let handler = new HttpClientHandler();
        let httpClient = new HttpClient(handler)

        let config = Configuration.Default
                                //.With(new HttpClientRequester(httpClient))
                                .WithDefaultLoader()
                                //.WithDefaultCookies()
                                .WithCss()
                                .WithJs()

        do
            handler.AllowAutoRedirect <- true
            handler.AutomaticDecompression<- DecompressionMethods.GZip ||| DecompressionMethods.Deflate
            handler.ClientCertificateOptions <- ClientCertificateOption.Automatic
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/W.X.Y.Z Mobile Safari/537.36 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)")
            httpClient.BaseAddress <- Uri(baseUrl)
        
        let saveToFiles data = fileSaverAgent.SaveToFiles data

        let linksProducer (parentUrl:string, url:string) =
            async{
                if not (linksDictionary.ContainsKey(url)) then
                  linksDictionary.[url] <- parentUrl
                  try
                    
                    if(linksDictionary.Count % 100 = 0 ) then fileSaverAgent.FlushFiles()

                    log ({LinksInQueue = (linksDictionary.Count + blockingCollection.Count);
                          CheckedLinks = linksDictionary.Count ;
                          Message = url})

                    use! response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken)|>Async.AwaitTask
                    if(response.IsSuccessStatusCode) then
                        if(url.StartsWith("http") && not (url.Contains(baseUrl))) then
                            //save file
                            saveToFiles (true, parentUrl, url)
                        else
                            let contentType = response.Content.Headers.ContentType.MediaType
                            if(contentType = "text/html") then
                                let! source = response.Content.ReadAsStringAsync() |>Async.AwaitTask       
                                use context = BrowsingContext.New(config)
                                let parser = context.GetService<IHtmlParser>()
                                use! doc = parser.ParseDocumentAsync(source)|>Async.AwaitTask
                                let links = 
                                        doc.QuerySelectorAll("a")
                                                   |> Seq.filter (fun x -> x.HasAttribute("href"))
                                                   |> Seq.map (fun x -> x.GetAttribute("href"))
                                                   |> Seq.filter (fun x -> (x.Length > 1) && not (x.StartsWith("javascript")))
                                                   |> Seq.map (fun x ->
                                                       let uri = (new Uri(url)).ToString()
                                                       let index = uri.LastIndexOf('/')
                                                       let path = uri.Substring(0, index)

                                                       if (x.StartsWith('/')) then $"{path}{x}"
                                                       elif (x.StartsWith "http") then x
                                                       else $"{path}/{x}")
                                                   |> Seq.toList

                                if (not (links.Any()) && (blockingCollection.Count = 0)) then
                                    blockingCollection.CompleteAdding()
                                    (fileSaverAgent :> IDisposable).Dispose()
                                    
                                else
                                    for link in links do
                                          let newLink =
                                                if(link.StartsWith("/")) then
                                                    $"{baseUrl}{link}"
                                                else if not (link.StartsWith("http")) then
                                                    $"{baseUrl}/{link}"
                                                else link

                                          if not(linksDictionary.ContainsKey(newLink)) then
                                              blockingCollection.Add((url, newLink))
                                    //save file
                                    saveToFiles (true, parentUrl, url)
                             else
                                 //save file                 
                                 saveToFiles (true,parentUrl, url)
                    else 
                          //save file
                          saveToFiles (false, parentUrl, url)
                  with
                    | :?  
                           OperationCanceledException -> 
                                                        blockingCollection.CompleteAdding()
                                                        (fileSaverAgent :> IDisposable).Dispose()


                    | _ ->  //save file
                           saveToFiles (false,parentUrl, url)
                }

        let consumer1() =
                async{
                       for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                           //use! holder = webRequestGate.AsyncAcquire() 
                           do! linksProducer (parentUrl,url)
                           //log (url)
                    }
        let consumer2() =
            async{
                   for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                       //use! holder = webRequestGate.AsyncAcquire() 
                       do! linksProducer (parentUrl,url)
                       //log (url)
                }
        let consumer3() =
            async{
                   for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                       //use! holder = webRequestGate.AsyncAcquire() 
                       do! linksProducer (parentUrl,url)
                       //log (url)
                }
        let consumer4() =
           async{
               for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                   //use! holder = webRequestGate.AsyncAcquire() 
                   do! linksProducer (parentUrl,url)
                   //log (url)
               }
        let consumer5() =
            async{
                   for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                       //use! holder = webRequestGate.AsyncAcquire() 
                       do! linksProducer (parentUrl,url)
                       //log (url)
                }

        interface System.IDisposable with 
            member _.Dispose() = 
                     blockingCollection.Dispose()
                     httpClient.Dispose()
                     handler.Dispose()
                     (fileSaverAgent :> IDisposable).Dispose()

        member _.Start() =
            
            consumer1() |> Async.Start
            consumer2() |> Async.Start
            consumer3() |> Async.Start
            consumer4() |> Async.Start
            consumer5() |> Async.Start
            
            linksProducer ("", url.ToString()) |> Async.StartAsTask
            
                     
                     

       
   

        



