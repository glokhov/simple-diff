module Tests

open SimpleDiff
open SimpleDiff.Diff
open Xunit

let oldStrings = [ "I"; "really"; "like"; "ice-cream" ]
let newStrings = [ "I"; "do"; "not"; "like"; "ice-cream" ]

let expectedStrings =
    [ Equal [ "I" ]; Delete [ "really" ]; Insert [ "do"; "not" ]; Equal [ "like"; "ice-cream" ] ]

let oldNumbers = [ 1.0; 2.0; 3.0; 4.0 ]
let newNumbers = [ 1.0; 2.1; 2.2; 3.0; 4.0 ]

let expectedNumbers = [ Equal [ 1.0 ]; Delete [ 2.0 ]; Insert [ 2.1; 2.2 ]; Equal [ 3.0; 4.0 ] ]

[<Fact>]
let ``test strings`` () =
    Assert.Equivalent(expectedStrings, getDiffs oldStrings newStrings)

[<Fact>]
let ``test numbers`` () =
    Assert.Equivalent(expectedNumbers, getDiffs oldNumbers newNumbers)
