[<AutoOpen>]
module DiggingGame

open System.IO

open ResultTypes
open PrintResultHelpers
open RecordsInMemory
open RecordsInDb
open ProcessUser
open RandomNumbers

type DiggingGame(what_file:string, where_file:string) = 
    
    let mutable filename_where = "digg_where.txt"
    let mutable filename_what = "digg_what.txt"

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
    let LoadSavedRecords =
        use conn = getConnection(db_name)
        DatabaseSelectAllRecords conn
            |> List.iter (fun r ->
                 diggRecAdd diggRecords r.what r.weight)

    /// Create a digg/movement from user.
    let MakeADigg (who:string) = 
        LoadSavedRecords
        
        let index_what = getRandom (len_what-1)
        let index_where = getRandom (len_where-1)

        let item_where = text_where.[index_where]
        let item_what = text_what.[index_what]

        let age = getNormalDistributionRandom()
    
        let p:UserAction = {
            who = who
            what = item_what
            where  = item_where
            value_new  = age
            }
        p 

    /// Make random items and process them.
    let processDigg (who:string) = async{
        let digg = MakeADigg who
        let rslt = ProcessFoundNotFound digg
        result <- rslt
    }

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
