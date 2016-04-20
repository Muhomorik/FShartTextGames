module RecordsInMemory

open System.Collections.Concurrent

let diggRecords = new ConcurrentDictionary<string, int>()

/// Adds new value to in-memory records or replaces if exists.
/// Returns inserted value.
let diggRecAddOrUpdate (dict :ConcurrentDictionary<string, int>) (what :string) (age :int) = 
    dict.AddOrUpdate(what, age, (fun _ _ -> age ))



let diggRecUpdate (dict :ConcurrentDictionary<string, int>) (what :string) (age_new :int) (age_stored :int) = 
    let res = dict.TryUpdate(what, age_new, age_stored)
    match res with
    | true -> 
        //printfn "\nupdate %s from %d to %d\n" what age_stored age_new
        ()
    | false -> failwith "diggRecUpdate failed"