module RecordsInDb

open System
open System.IO
open System.Data.SQLite
open Dapper
open ResultTypes

/// Table ORM class.
type recordsFishingTable = {
     nickname : string 
     what :string
     weight :int
     dateWhen :DateTime
} 

/// COnvert Result to DB  ORM type
/// TODO: test
let ConvertResultToRecord (res:Result) = 
    let p:recordsFishingTable = {
        nickname = res.nickname 
        what = res.what
        weight = res.value_new
        dateWhen = DateTime.UtcNow    
        }
    p

/// Database name.
[<Literal>]
let db_name = "recordsDb.db"

// TODO: its a digging game...  eename table

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
    lazy(
    printfn "Create default table"
    let res = dbConnection.Execute(query_TableCreateDigging)
    if res <> 0 then failwith "Failed to create defailt table."
    )

/// Get database if exists or create if missing.
let GetDatabase db_name =
    if(not (File.Exists(db_name))) 
    then 
        printfn "Create new db %s " db_name
        SQLiteConnection.CreateFile(db_name)
        false
    else
        //printfn "db exists"
        true

/// Get db connection.
let getConnection(db_name:string) = 
    // create if missing.
    let existed = GetDatabase db_name

    let conString = sprintf "Data Source=%s;Version=3;" db_name
    //printfn "Connection string: %s " conString

    let dbConnection = new SQLiteConnection(conString)

    if not existed then (CreateDefaultTable dbConnection).Force()
    dbConnection

/// Query for DB insert.
let query_dbInsert =
    @"INSERT OR REPLACE INTO recordsFishing VALUES (@nickname, @what, @weight, @dateWhen)"


/// Insert new value into DB.
let DatabaseInsert (dbConnection:SQLiteConnection)(record :recordsFishingTable) =
    let cnt = dbConnection.Execute(query_dbInsert, record)
    cnt

// TODO: select is only used for global load at start

/// Query for DB SELECT.
let query_dbSelectRecord what =
    sprintf @"select * from recordsFishing where what='%s' ORDER BY weight DESC" what
     
/// Insert new value into DB.
let DatabaseSelect (dbConnection:SQLiteConnection)(what :string) =
    let q = query_dbSelectRecord what
    let record = dbConnection.Query<recordsFishingTable>(q)
    record

// TODO: test DatabaseSelectAllRecords
/// Query for DB SELECT All records.
let query_dbSelectAllRecords =
    @"select * from recordsFishing ORDER BY weight DESC"
     
/// Insert new value into DB. Returns list of db objects.
let DatabaseSelectAllRecords (dbConnection:SQLiteConnection) =
    let q = query_dbSelectAllRecords
    let record = dbConnection.Query<recordsFishingTable>(q)
    record |> List.ofSeq

/// Query for DB UPDATE.
let query_dbUpdate =
    sprintf @"UPDATE recordsFishing 
        SET nickname = @nickname, what = @what, weight = @weight, dateWhen = @dateWhen  
        WHERE what = @what"
     
/// Insert new value into DB.
let DatabaseUpdate (dbConnection:SQLiteConnection)(record :recordsFishingTable) =
    let q = query_dbUpdate
    let cnt = dbConnection.Execute(q, record)
    cnt

/// Insert new value into DB or update existing.
/// TODO: test DatabaseInsertOrUpdate
let DatabaseInsertOrUpdate (dbConnection:SQLiteConnection)(record :recordsFishingTable) =
    //printfn "InsertOrUpdate"
    // TODO: check db logic after updating everything to what as PK. Must not need this function.
    let selcted = DatabaseSelect dbConnection record.what |> Seq.length

    match selcted with
    | 0 -> DatabaseInsert dbConnection record
    | _ -> DatabaseUpdate dbConnection record

//     nickname : string 
//     what :string
//     weight :int
//     dateWhen :strin