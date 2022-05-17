module WebCrawlerLib

open System
open System.Net
open System.Net.Http
open AngleSharp
open Microsoft.Isam.Esent.Collections.Generic
open System.Threading
open System.IO
open AngleSharp.Html.Parser

let linksDictionary = new PersistentDictionary<string, string>("Links")
let queueDictionary = new PersistentDictionary<string, string>("Queue")

ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true)
ServicePointManager.DefaultConnectionLimit <- 16
ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Tls11 ||| SecurityProtocolType.Tls

let isHTTPFamily (url : Uri) = url.Scheme = Uri.UriSchemeHttp || url.Scheme = Uri.UriSchemeHttps

let handler = new HttpClientHandler()
handler.AllowAutoRedirect <- true;
handler.AutomaticDecompression <- DecompressionMethods.GZip|||DecompressionMethods.Deflate
handler.ClientCertificateOptions <- ClientCertificateOption.Automatic

let httpClient = new HttpClient(handler)
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 5.0; SM-G920A) AppleWebKit (KHTML, like Gecko) Chrome Mobile Safari (compatible; AdsBot-Google-Mobile; +http://www.google.com/mobile/adsbot.html)")

let config = Configuration.Default
                          .WithDefaultLoader()
                          .WithCss()
                          .WithJs()
let mutable BASEURL = ""
let outputDir = "Output"
let goodFile = Path.Combine(outputDir, "GoodLinks.csv");
let badFile = Path.Combine(outputDir, "BadLinks.csv");
     
let getLinks (parent:string) (url:Uri) (cancellationToken:CancellationToken) =
    async{
     try
        use! response = httpClient.GetAsync(url, 
                                            HttpCompletionOption.ResponseHeadersRead, 
                                            cancellationToken)|>Async.AwaitTask
        if(response.IsSuccessStatusCode) then
           let contentType = response.Content.Headers.ContentType.MediaType
           if (not (url.Contains(BASEURL)) ||  (contentType <> "text/html")) then 
              //External link
              //save
              return Ok (Some [])
            else 
               //Internal Link
               //check absolut link or not?
                if(contentType = "text/html") then
                   //html page
                    let! source = response.Content.ReadAsStringAsync() |>Async.AwaitTask       
                    use context = BrowsingContext.New(config)
                    let parser = context.GetService<IHtmlParser>()
                    use! doc = parser.ParseDocumentAsync(source)|>Async.AwaitTask
                    let links = 
                             doc.QuerySelectorAll("a")
                             |> Seq.filter(fun x -> x.HasAttribute("href"))
                             |> Seq.map (fun x -> x.GetAttribute("href"))
                             |> Seq.filter(fun x -> (x.Length > 1) && not (x.StartsWith("javascript")))
                             |> Seq.map (fun (x:string) -> 
                                                if (x.StartsWith("http")) then x
                                                elif (x.StartsWith("/")) then 
                                                     let u = url.ToString()
                                                     let subStr = u.Substring(0, u.LastIndexOf('/'))
                                                     $"{subStr}{x}"
                                                else
                                                     let u = url.ToString()
                                                     $"{u}{x}"
                                                     

                             |> Seq.toList
                    return Ok (Some links)
                else
                  //non html page 
                   return Ok (Some [])
         else//No success code
               return Ok None
        
     with
        | ex -> return Error (ex)   
    }

let checker (startingUrl:string) =
    async {
            
    
          }|> Async.StartAsTask
    

