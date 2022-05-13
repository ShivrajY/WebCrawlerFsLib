open WebCrawlerFsLib.WebCrawler
open System.Threading.Tasks
open System

let url = "https://eclipse2024.org"

setBaseUrl url
consumer() |> Async.Start
linksProducer ("", url) |> Async.RunSynchronously |> ignore

Console.ReadLine()|>ignore


