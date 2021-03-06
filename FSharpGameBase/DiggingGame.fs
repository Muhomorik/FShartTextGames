﻿[<AutoOpen>]
module DiggingGame

open System.IO

open ResultTypes
open PrintResultHelpers
open RecordsInMemory
open RecordsInDb
open ProcessUser
open RandomNumbers

type DiggingGame(whatFile:string, whereFile:string) = 
    
    let mutable filename_where = whereFile
    let mutable filename_what = whatFile

    let text_where = File.ReadAllLines(filename_where)
    let text_what = File.ReadAllLines(filename_what)

    let len_where = text_where.Length
    let len_what = text_what.Length

    /// Current result, gets replaced every run. Used by printing.
    let mutable result:Result =
        { 
        result = ResultType.FoundNothing
        nickname = ""
        what = ""
        where = ""
        value_old = 0
        value_new = 0
        }

    /// Load db records to memory storage.
    let loadSavedRecords = async{
        match diggRecords.Count with
        | 0 ->
            printfn "Re-load records..."
            use conn = getConnection(dbName)
            DatabaseSelectAllRecords conn
                |> List.iter (fun r ->
                     diggRecAddOrUpdate r.what r.weight |> ignore)
        | _ -> ()
    }
    
    // Ctor call, load db.
    do loadSavedRecords |> Async.RunSynchronously

    /// Create a digg/movement from user.
    let makeDigg (who:string) = async{
        let index_what = getRandom (len_what-1)
        let index_where = getRandom (len_where-1)

        let item_where = text_where.[index_where]
        let item_what = text_what.[index_what]

        let age = getNormalDistributionRandom() // this one is slow.
    
        let p:UserAction = {
            who = who
            what = item_what
            where  = item_where
            value_new  = age
            }
        return p
    }     

    /// Make random items and process them.
    let processDigg (who:string) = 
        let digg = makeDigg who |> Async.RunSynchronously
        let rslt = ProcessFoundNotFound digg 20
        // TODO: write db  here?
        result <- rslt
    

    /// User action lile digg.
    member this.MakeAction(who:string) = processDigg who
                                                                   
    /// Get text to print before action. Must call efter MakeAction().
    member this.GetPreText where = diggStart result.where

    /// Get text to print after action. Must call efter MakeAction().
    member this.GetPostText where = printResult result

    /// Where filename.
    member this.WhereFile 
        with get() = filename_where 
        and private set(value) =  filename_where <- value

    /// What filename.
    member this.FileWhat 
        with get() = filename_what 
        and private set(value) =  filename_what <- value
