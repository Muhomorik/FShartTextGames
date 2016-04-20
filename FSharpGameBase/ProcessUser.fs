module ProcessUser

open ResultTypes
open RecordsInMemory
open RecordsInDb

// TODO: Use record for everything? Like 'diggRecUpdate record' 
// TODO: Figure out types Result/DbResult. 

/// Process and compare existing values.
let ProcessExistingValues val_new val_stored who what where = 
    match val_new > val_stored with
    // New record. Hurray!
    | true ->
        diggRecUpdate diggRecords what val_new val_stored
        let r = { 
            result = ResultType.FoundRecord
            nickname = who
            what = what
            where = where
            value_old = val_stored
            value_new = val_new
            }

        // Update db
        use conn = getConnection(db_name)
        let orm_res = ConvertResultToRecord r
        DatabaseInsertOrUpdate conn orm_res |> ignore
        r

    // Nothing new, skip.
    | false ->
        { 
        result = ResultType.Found
        nickname = who
        what = what
        where = where
        value_old = val_stored
        value_new = val_new
        }

/// Check if result been seen before or completely new.
let ProcessForFound (movement:UserAction) = 
    
    // Get record from memory store.
    let ok, stored = diggRecords.TryGetValue(movement.what)
    match ok with
    // Value have already been seen.     
    | true ->
        ProcessExistingValues movement.value_new stored movement.who movement.what movement.where
    
    // New value.
    | false ->
        diggRecAdd diggRecords movement.what movement.value_new // TODO: take Result
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
        use conn = getConnection(db_name)
        let orm_res = ConvertResultToRecord r
        DatabaseInsertOrUpdate conn orm_res |> ignore
        r

/// Top check - found or notheng found.
let ProcessFoundNotFound (movement:UserAction) = 
    match (movement.value_new) with
    // Got smth if weight is over 10.
    | i when i >= 20 -> 
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
