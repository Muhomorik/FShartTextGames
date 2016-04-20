module RecordsInMemory

open System.Collections.Concurrent

let diggRecords = new ConcurrentDictionary<string, int>()

// Abstractions from implementation (in case want to change later).

/// Adds new value to in-memory records or replaces if exists.
/// Returns inserted value.
let diggRecAddOrUpdate (what :string) (age :int) = 
    diggRecords.AddOrUpdate(what, age, (fun _ _ -> age ))

/// Try to get value from in-memory storage.
let diggRecTryGetValue (what :string)  = 
    diggRecords.TryGetValue(what)