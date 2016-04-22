module DbInit

open System.IO
open System.Data.SQLite
open Dapper

/// Query to create Digg records table.
let query_TableCreateDigging =
    @"CREATE TABLE recordsFishing (
    nickname TEXT         NOT NULL,
    what     TEXT         NOT NULL
                          PRIMARY KEY,
    weight   INT          NOT NULL,
    dateWhen DATETIME     NOT NULL
);"

/// Create default db tables for empty db.
let CreateDefaultTable (dbConnection:SQLiteConnection) = 
    // TODO: check if table eists
    // SELECT * FROM sqlite_master WHERE name ='myTable' and type='table'; 
    lazy(
    printfn "Create default table"
    let res = dbConnection.Execute(query_TableCreateDigging)
    if res <> 0 then failwith "Failed to create defailt table."
    )

/// Get database if exists or create if missing.
let GetDatabase dbNname =
    if(not (File.Exists(dbNname))) 
    then 
        printfn "Create new db %s " dbNname
        SQLiteConnection.CreateFile(dbNname)
        false
    else
        //printfn "db exists"
        true
