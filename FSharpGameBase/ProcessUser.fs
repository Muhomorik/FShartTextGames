module ProcessUser

open ResultTypes
open RecordsInMemory
open RecordsInDb

// TODO: Use record for everything? Like 'diggRecUpdate record' 
// TODO: Move db write up. Hard to test now. 
// TODO: Figure out types Result/DbResult. 


/// Add new record to db record table async.
let AddDiggRecordAsync (r:Result) = async{
    // Write to db
    use conn = getConnection(dbName)
    let orm_res = ConvertResultToRecord r
    DatabaseInsertOrUpdate conn orm_res |> ignore
}

///Update DB record table.
let UpdateRecordAsync (r:Result) = async{
    // Update db
    use conn = getConnection(dbName)
    let orm_res = ConvertResultToRecord r
    DatabaseInsertOrUpdate conn orm_res |> ignore
}

/// Process and compare existing values. Result must come with type Found.
let ProcessExistingValues (r:Result) =
    match r.value_new > r.value_old with
    // New record. Hurray!
    | true ->
        diggRecAddOrUpdate r.what r.value_new |> ignore  // This is better in main thread, not async.
        UpdateRecordAsync r |> Async.Start
        let rUpdate = {r with result = ResultType.FoundRecord}
        rUpdate
    // Nothing new, skip.
    | false ->
        r  // must have right ResultType from input (Found).

/// Check if result been seen before or completely new.
let ProcessForFound (movement:UserAction) = 

    // Get record from memory store.
    match diggRecTryGetValue movement.what with
    // Value have already been seen.     
    | true, stored ->
        let r = 
            { 
            result = ResultType.Found // this one is temp.
            nickname = movement.who
            what = movement.what
            where = movement.where
            value_old = stored
            value_new = movement.value_new
            }
        ProcessExistingValues r

    // New value.
    | false, _ ->      
        diggRecAddOrUpdate movement.what movement.value_new |> ignore
        let r = 
            { 
            result = ResultType.FoundNew
            nickname = movement.who
            what = movement.what
            where = movement.where
            value_old = 0
            value_new = movement.value_new
            }
        
        // Write to db        
        AddDiggRecordAsync r |> Async.Start 
        r

/// Top check - found or nothing found based on weight.
let ProcessFoundNotFound (movement:UserAction) (treshold:int) = 
    match (movement.value_new) with
    // Got smth if weight is over 10.
    | i when i >= treshold -> 
        ProcessForFound movement
    // Nothing found for others.
    | _ -> 
        { 
        result = ResultType.FoundNothing
        nickname = movement.who
        what = movement.what
        where = movement.where
        value_old = 0
        value_new = movement.value_new
        }
