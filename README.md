## Simple Diff [![Nuget Version](https://img.shields.io/nuget/v/SimpleDiff)](https://www.nuget.org/packages/SimpleDiff)

F# implementation of a diffing algorithm ported from https://github.com/gjaldon/simple-diff.

### Getting started

```shell
dotnet add package SimpleDiff
```

### Usage

```fsharp
open SimpleDiff

let oldStrings = [ "I"; "really"; "like"; "ice-cream" ]
let newStrings = [ "I"; "do"; "not"; "like"; "ice-cream" ]

getDiffs oldStrings newStrings
```

Evaluates to

```fsharp
[ Equal [ "I" ]
  Delete [ "really" ]
  Insert [ "do"; "not" ]
  Equal [ "like"; "ice-cream" ] ]
```

#### Can be used with any comparable type

```fsharp
open SimpleDiff

let oldNumbers = [ 1.0; 2.0; 3.0; 4.0 ]
let newNumbers = [ 1.0; 2.1; 2.2; 3.0; 4.0 ]

getDiffs oldNumbers newNumbers
```

Evaluates to

```fsharp
[ Equal [ 1.0 ]
  Delete [ 2.0 ]
  Insert [ 2.1; 2.2 ]
  Equal [ 3.0; 4.0 ] ]
```