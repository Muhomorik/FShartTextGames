module PrintResultHelpers

open System
open ResultTypes

/// Cheering words.
let goodWords = [ "хорошая"; "отличная"; "достойная"]  

/// Ramdomness.
let rnd = System.Random()

/// Random cheer words from list.
let getRanomGoodWord() =
    let index = rnd.Next(0, goodWords.Length-1)
    goodWords.[index]

/// Get age ending for number in Russian.
let ageToLetters age = 
    // https://www.lingq.com/learn/ru/preview/item/55200/
    let str = age.ToString()

    match str with
    | "1" -> "год"
    | "2" | "3"| "4" -> "года"
    | "5" | "6"| "7"| "8"| "9"| "10" | "11" | "12" | "13" | "14" | "15" | "16" | "17" | "18" | "19" | "20" -> "лет"
    | i when i.EndsWith("0") -> "лет"
    | i when i.EndsWith("1") -> "год"
    | i when i.EndsWith("2") -> "года"
    | i when i.EndsWith("3") -> "года"
    | i when i.EndsWith("4") -> "года"
    | i when i.EndsWith("5") -> "лет"
    | i when i.EndsWith("6") -> "лет"
    | i when i.EndsWith("7") -> "лет"
    | i when i.EndsWith("8") -> "лет"
    | i when i.EndsWith("9") -> "лет"
    | _ -> "года"

/// Print digg start phrase.
let diggStart (where:string) = 
    sprintf "Вы начали раскопки %s и усиленно роете лопатами, экскаватором... Вам кажется что ваш совочек ударился обо что-то твердое. Може это клад?!" where

/// Digg successfull phrase.
let diggSuccess (what:string) (age:int) = 
    sprintf "Вы только что выкопали %s, возраст - %d %s" what age (ageToLetters age)

/// Digg, nothing fund phrase.
let diggFailure (where:string) (who:string) = 
    sprintf "По уши закопавшись %s, @%s, нифига вы не выкопали! Может повезёт в другом месте?" where who

/// New record phrase.
let diggNewRecord (who:string) = 
    sprintf "Ого!!! Это новый рекорд! Так держать, %s!" who

/// Digg, not a record, missing to record phrase.
let diggNotRecord (weight_old:int) (weight_new:int) = 
    let diff = weight_old - weight_new
    sprintf "Но до рекорда не хватило - %d %s. Однако, попытка %s!" diff (ageToLetters diff) (getRanomGoodWord())

/// Get Result obj as string for printing.
let printResult (res:Result) = 
    match res.result with
    | FoundRecord ->  
        let success = diggSuccess res.what res.value_new
        let msg = diggNewRecord res.nickname
        sprintf "%s\n%s" success msg
    
    | Found -> 
        let success = diggSuccess res.what res.value_new
        let msg = diggNotRecord res.value_old res.value_new
        sprintf "%s\n%s" success msg        
        
    | FoundNew -> 
        diggSuccess res.what res.value_new

    | FoundNothing -> 
        diggFailure res.where res.nickname

