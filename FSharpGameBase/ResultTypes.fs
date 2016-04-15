module ResultTypes

/// Aser action type for: digg, fishing or hunting.
type UserAction = { 
    who :string
    what: string
    where :string
    value_new : int 
    }

/// Type of result.
type ResultType = FoundRecord | Found |  FoundNew | FoundNothing

/// Resulting values.
type Result = { 
    result: ResultType
    nickname :string
    what: string
    where :string
    value_old : int
    value_new : int 
    }