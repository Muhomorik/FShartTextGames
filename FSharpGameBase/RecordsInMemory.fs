module RecordsInMemory

open System
open System.Collections.Concurrent

let diggRecords = new ConcurrentDictionary<string, int>()

let diggRecInsert (dict :ConcurrentDictionary<string, int>) (what :string) (age :int) = 
    dict.AddOrUpdate(what, age, (fun _ _ -> age ))

let diggRecAdd (dict :ConcurrentDictionary<string, int>) (what :string) (age :int) = 
    let res = dict.TryAdd(what, age)
    match res with
    | true -> 
        //printfn "Add new %s size: %d" what dict.Count
        ()
    | false -> failwith "diggRecAdd failed"

let diggRecUpdate (dict :ConcurrentDictionary<string, int>) (what :string) (age_new :int) (age_stored :int) = 
    let res = dict.TryUpdate(what, age_new, age_stored)
    match res with
    | true -> 
        //printfn "\nupdate %s from %d to %d\n" what age_stored age_new
        ()
    | false -> failwith "diggRecUpdate failed"