// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Text

[<EntryPoint>]
let main argv = 
    Console.OutputEncoding <- Encoding.Unicode

    let stopWatch = System.Diagnostics.Stopwatch.StartNew()

    let diggClass = new DiggingGame("digg_what.txt", "digg_where.txt")

    [1..20] |> List.iter (fun x  -> 
        diggClass.MakeAction(x.ToString())

        printfn "%s" (diggClass.GetPreText())
        printfn "%s" (diggClass.GetPostText())
        )
    
    stopWatch.Stop()
    printfn "%f" stopWatch.Elapsed.TotalMilliseconds

    0 // return an integer exit code
