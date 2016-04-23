module TestDatabase

open NUnit.Framework
open FsUnit
open System
open System.IO

let rec1: RecordsInDb.recordsFishingTable = {
     nickname = "n1" 
     what = "what 1"
     weight = 20
     dateWhen = DateTime.Today
}

let rec1_updated: RecordsInDb.recordsFishingTable = {
     nickname = "n2" 
     what = "what 1"
     weight = 22
     dateWhen = DateTime.Today
}



[<Test>]
let``test db INSERT new``()=
    
    let workdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let fullPath = Path.Combine(workdir, RecordsInDb.dbName)
    File.Delete(fullPath)

    use conn = RecordsInDb.getConnection(fullPath)
    let cnt = RecordsInDb.DatabaseInsert conn rec1   
    
    cnt |> should equal 1

    File.Delete(fullPath)


[<Test>]
let``test db SELECT new``()=
    
    let workdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let fullPath = Path.Combine(workdir, RecordsInDb.dbName)
    File.Delete(fullPath)

    use conn = RecordsInDb.getConnection(fullPath)
    
    // Make dummy insert
    let cnt = RecordsInDb.DatabaseInsert conn rec1   
    
    cnt |> should equal 1 // or insert failed.

    // Select and check that insert is the same.
    let record = RecordsInDb.DatabaseSelect conn rec1.what |> List.ofSeq

    record.Head.nickname |> should equal rec1.nickname 
    record.Head.what |> should equal rec1.what 
    record.Head.weight |> should equal rec1.weight 
    record.Head.dateWhen |> should equal rec1.dateWhen 

    File.Delete(fullPath)

[<Test>]
let``test db UPDATE new``()=
    
    let workdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let fullPath = Path.Combine(workdir, RecordsInDb.dbName)
    File.Delete(fullPath)

    use conn = RecordsInDb.getConnection(fullPath)
    
    // Make dummy insert
    let cnt = RecordsInDb.DatabaseInsert conn rec1   
    
    cnt |> should equal 1 // or insert failed.

    // Select and check that insert is the same.
    let cnt = RecordsInDb.DatabaseUpdate conn rec1_updated

    cnt |> should equal 1 

    File.Delete(fullPath)



//     nickname : string 
//     what :string
//     weight :int
//     dateWhen :strin