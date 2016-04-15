module RandomNumbers

open System
open MathNet.Numerics.Distributions // Random distribution.

let rnd = System.Random()

let getRandom maxNum = 
    rnd.Next(0, maxNum)

// http://numerics.mathdotnet.com/Random.html
let getNormalDistributionRandom() = 
    Normal.Sample(14.0, 5.0) |> Convert.ToInt32 


