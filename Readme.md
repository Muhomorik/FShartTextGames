# Note #

This is a fork of an old IRC game !gidd (there is a !hund, !fish also).

Mostly in Russian :D

Original script belongs to Windrop clan, [windrop.clan.su/update/games/fish.tcl](windrop.clan.su/update/games/fish.tcl) and is written in TCL. Kept the test strings, everything else is new.

[Tcl Tutorial](https://www.tcl.tk/man/tcl8.5/tutorial/tcltutorial.html)

TODO: 

- [ ] write readme
- [ ] other games, hunt, fist, etc
- [ ] english text files (russiang endings are hardcoded)
- [ ] more tests, mostly db.

## Used packages ##

- [Dapper](https://github.com/StackExchange/dapper-dot-net) - a simple object mapper for .Net
- System.Data.SQLite.Core, SQLite database to store results between sessions.
- [Math.NET Numerics](http://numerics.mathdotnet.com/#Math-NET-Numerics) for FSharp. Generate random numbers with Normal Distribution. 

## Whats in ##

- *FSharpGameBase* - base library.
- *FSharpGamesBaseTest* - Unint tests using FsUnit/NUnit for logic
- *FSharpTextGames* - simple game ap that prints N randoms.

## How to ##

Requires:

- *digg_what.txt* list of possible items to digg.
- *digg_where.txt* list of possible locations to use.
- Base library, *FSharpGameBase.dll*,

Records are stored in recordsDb.db. Will be created if missing.

Example:

```F#
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
```

Example output:

Not a record:

    Вы начали раскопки на кладбище и усиленно роете лопатами, экскаватором... Вам кажется что ваш совочек ударился обо что-то твердое. Може это клад?!
    Вы только что выкопали деревянную вилку, возраст - 10 лет
    Но до рекорда не хватило - 7 лет. Однако, попытка хорошая!

Nothing found: 

    Вы начали раскопки на том свете и усиленно роете лопатами, экскаватором... Вам кажется что ваш совочек ударился обо что-то твердое. Може это клад?!
    По уши закопавшись на том свете, @3, нифига вы не выкопали! Может повезёт в другом месте?

New record:

    Вы начали раскопки в цветочном горшке и усиленно роете лопатами, экскаватором... Вам кажется что ваш совочек ударился обо что-то твердое. Може это клад?!
    Вы только что выкопали деревянную вилку, возраст - 18 лет
    Ого!!! Это новый рекорд! Так держать, 4!
