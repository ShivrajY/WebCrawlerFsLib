open WebCrawlerFsLib.WebCrawler
open System.Threading.Tasks
open System
open System.Threading

let url = "https://eclipse2024.org"

let cts = new CancellationTokenSource()

let w = new WebCrawler(Uri(url), "allLinks.csv", "goodLinks.csv", "badLinks.csv",cts.Token, (printfn "%s"))
w.Start().Wait()
printfn "Readline wait"
Console.ReadLine()|>ignore
// task cancelled
cts.Cancel()
//(w:>IDisposable).Dispose()
Console.ReadLine()|>ignore


