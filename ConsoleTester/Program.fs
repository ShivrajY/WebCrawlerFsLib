open WebCrawlerFsLib.WebCrawler
open System.Threading.Tasks
open System
open System.Threading

let url = "https://eclipse2024.org"

let cts = new CancellationTokenSource()

let w = new Crawler(Uri(url), "allLinks.csv", "goodLinks.csv", "badLinks.csv",cts.Token, (printfn "%A"))
w.Start().Wait()
printfn "Readline wait"
Console.ReadLine()|>ignore
// task cancelled
cts.Cancel()
//(w:>IDisposable).Dispose()
Console.ReadLine()|>ignore


