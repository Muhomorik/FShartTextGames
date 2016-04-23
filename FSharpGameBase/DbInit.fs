module DbInit

open System
open System.IO
open System.Data.SQLite
open Dapper

/// Query arg as string.
type QueryString = { ArgStr: string }   // doesn't work when private.

/// Query to create Digg records table.
let query_TableCreateDigging =
    @"CREATE TABLE recordsFishing (
    nickname TEXT         NOT NULL,
    what     TEXT         NOT NULL
                          PRIMARY KEY,
    weight   INT          NOT NULL,
    dateWhen DATETIME     NOT NULL
);"

/// Check if db table exists query.
let queryTableExists = 
    @"SELECT Count(*) FROM sqlite_master WHERE name= @ArgStr and type='table'" 

/// Create default db tables for empty db.
let CheckIfTableExists (dbConnection:SQLiteConnection) = 
    //let q = queryTableExists "recordsFishing"
    let res = dbConnection.Query<int>(queryTableExists, {ArgStr="recordsFishing"})
    res |> Seq.exactlyOne


/// Create default db tables for empty db.
let CreateDefaultTable (dbConnection:SQLiteConnection) = 
    
    let tableExists = CheckIfTableExists dbConnection
    match tableExists with
    | 1 -> 
        //printfn "Table exists, skip."
        () 
    | _ ->   
        //printfn "Create default table"
        let res = dbConnection.Execute(query_TableCreateDigging)
        if res <> 0 then failwith "Failed to create defailt table."
        ()


/// Get database if exists or create if missing.
let CreateDatabaseIfMissing dbNname =
    if(not (File.Exists(dbNname))) 
    then 
        printfn "Create new db %s " dbNname
        SQLiteConnection.CreateFile(dbNname)
        
        let conString = sprintf "Data Source=%s;Version=3;" dbNname
        let dbConnection = new SQLiteConnection(conString)
        CreateDefaultTable dbConnection
        false
    else
        //printfn "db exists"
        true
