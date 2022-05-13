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

let webRequestGate = RequestGate(10)

ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true)
ServicePointManager.DefaultConnectionLimit <- LIMIT
ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Tls11 ||| SecurityProtocolType.Tls


let handler = new HttpClientHandler();
handler.AllowAutoRedirect <- true
handler.AutomaticDecompression<- DecompressionMethods.GZip ||| DecompressionMethods.Deflate
handler.ClientCertificateOptions <- ClientCertificateOption.Automatic

let httpClient = new HttpClient(handler)
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/W.X.Y.Z Mobile Safari/537.36 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)")
 
let config = Configuration.Default
                //.With(new HttpClientRequester(httpClient))
                .WithDefaultLoader()
                //.WithDefaultCookies()
                .WithCss()
                .WithJs()

let linksDictionary = new ConcurrentDictionary<string,string>()             
let blockingCollection = new BlockingCollection<string*string>()

let mutable BaseUrl = "" 

let setBaseUrl baseUrl = BaseUrl <- baseUrl
                         httpClient.BaseAddress <- Uri(baseUrl)

let log msg = printfn "%A" msg
let saveToFiles (data:(bool * string * string)) = 
    log "Saving file..."
    log data
let linksProducer (parentUrl:string, url:string) =
    async{
        

        if not (linksDictionary.ContainsKey(url)) then
          linksDictionary.[url] <- parentUrl
          log "Inside producer"
          try
            log ("Link: " + url)
            use! response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)|>Async.AwaitTask
            if(response.IsSuccessStatusCode) then
                log ("Successful")
                if(url.StartsWith("http") && not (url.Contains(BaseUrl))) then
                    //save file
                    saveToFiles (true, parentUrl, url)
                else
                    let contentType = response.Content.Headers.ContentType.MediaType
                    log contentType
                    if(contentType = "text/html") then
                        let! source = response.Content.ReadAsStringAsync() |>Async.AwaitTask       
                        use context = BrowsingContext.New(config)
                        let parser = context.GetService<IHtmlParser>()
                        use! doc = parser.ParseDocumentAsync(source)|>Async.AwaitTask
                        let links = 
                                 doc.QuerySelectorAll("a")
                                 |> Seq.filter(fun x -> x.HasAttribute("href"))
                                 |> Seq.map (fun x -> x.GetAttribute("href"))
                        log ("Total Links: "+ links.Count().ToString())
                        for link in links do
                              let newLink =
                                            if(link.StartsWith("/")) then
                                                $"{BaseUrl}{link}"
                                            else if not (link.StartsWith("http")) then
                                                $"{BaseUrl}/{link}"
                                            else link
                              blockingCollection.Add((url,newLink))
                        //save file

                        saveToFiles (true, parentUrl, url)
                     else //Content type is not html but successful
                              //save file                 
                         saveToFiles (true,parentUrl, url)
            else 
                  //save file
                  saveToFiles (false, parentUrl, url)
          with
            |_ ->  //save file
                   saveToFiles (false,parentUrl, url)
        }

let consumer() =
        async{
               log "Inside consumer"
               let taskList = ResizeArray()
               for (parentUrl, url) in blockingCollection.GetConsumingEnumerable() do
                    taskList.Add(linksProducer (parentUrl,url))
                    if(taskList.Count > 10) then
                        taskList.ToArray()|>Async.Parallel|>Async.RunSynchronously|>ignore
                    
            }


       
   

        



