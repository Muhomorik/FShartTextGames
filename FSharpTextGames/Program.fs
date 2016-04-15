// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Text

[<EntryPoint>]
let main argv = 
    Console.OutputEncoding <- Encoding.Unicode

    let diggClass = new DiggingGame("digg_what", "digg_where.txt")

    let xlist = [1..20] |> List.iter (fun x  -> 
        diggClass.MakeAction(x.ToString())

        printfn "%s" (diggClass.GetPreText())
        printfn "%s" (diggClass.GetPostText())
        )
    
    0 // return an integer exit code
