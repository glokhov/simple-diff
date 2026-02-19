module SimpleDiff

type Diff<'T when 'T: comparison> =
    | Delete of 'T list
    | Insert of 'T list
    | Equal of 'T list

[<CompiledName("GetDiffs")>]
val getDiffs: oldItems: 'T list -> newItems: 'T list -> Diff<'T> list
