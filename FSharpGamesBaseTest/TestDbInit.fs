module TestDbInit

open NUnit.Framework
open FsUnit
open System
open System.IO

[<Test>]
let``bd is created if missing``()=
    
    let workdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let fullPath = Path.Combine(workdir, RecordsInDb.dbName)

    File.Delete(fullPath) // delete before.

    let exists_before = File.Exists(fullPath)
    let rslt = DbInit.CreateDatabaseIfMissing fullPath
    let exists_after = File.Exists(fullPath)

    // Check that table exists after db creation.
    let dbConnection = RecordsInDb.getConnection fullPath
    let tableExists = DbInit.CheckIfTableExists dbConnection
       
    exists_before |> should be False
    exists_after |> should be True
    
    rslt |> should be False
    tableExists |> should equal 1

    File.Delete(fullPath)

[<Test>]
let``db is reused when exists``()=
    
    let workdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let fullPath = Path.Combine(workdir, RecordsInDb.dbName)

    File.Delete(fullPath) // delete before.

    let rslt = DbInit.CreateDatabaseIfMissing fullPath
    let rslt2 = DbInit.CreateDatabaseIfMissing fullPath
        
    rslt |> should be False
    rslt2 |> should be True

    File.Delete(fullPath)


