// Shri Ganesha //
module WebCrawlerFsLib.WebCrawler

open System
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
let blockingCollection = new BlockingCollection<string>()

let linksProducer (url:string) =
    task {

          try
            use! response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
            if(response.IsSuccessStatusCode) then
                let contentType = response.Content.Headers.ContentType.MediaType
                if(contentType = "text/html") then
                    let! source = response.Content.ReadAsStringAsync()        
                    use context = BrowsingContext.New(config)
                    let parser = context.GetService<IHtmlParser>()
                    use! doc = parser.ParseDocumentAsync(source)
                    let links = 
                             doc.QuerySelectorAll("a")
                             |> Seq.filter(fun x -> x.HasAttribute("href"))
                             |> Seq.map (fun x -> x.GetAttribute("href"))

                    for link in links do
                        if not (linksDictionary.ContainsKey(link)) then
                          linksDictionary.[link] <- url
                          blockingCollection.Add(link)
                    return true
                else return true
            else return false
          with
            |_ ->  return false

        }
    
let consumer() =
    task{
           for url in blockingCollection.GetConsumingEnumerable() do
              let! result = linksProducer url   
              if (result) then () 
                //write to the good file
              else ()
                //write to the bad file

        }

        



