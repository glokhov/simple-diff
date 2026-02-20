namespace SimpleDiff

open System.Collections.Generic

type Diff<'T> =
    | Delete of 'T list
    | Insert of 'T list
    | Equal of 'T list

type SubsequenceInfo =
    { SubStartNew: int
      SubStartOld: int
      LongestSubsequence: int }

module Diff =
    let itemsCounter (items: 'T list) : Map<'T, int list> =
        items
        |> List.mapi (fun index item -> index, item)
        |> List.fold
            (fun map (index, item) ->
                let indices = Map.tryFind item map |> Option.defaultValue []
                Map.add item (index :: indices) map)
            Map.empty

    let getLongestSubsequence (oldItems: 'T list) (newItems: 'T list) : SubsequenceInfo =
        let oldItemsCounter = itemsCounter oldItems
        let overlap = Dictionary<int, int>()
        let mutable subStartOld = 0
        let mutable subStartNew = 0
        let mutable longestSubsequence = 0

        newItems
        |> List.iteri (fun newIndex newItem ->
            Map.tryFind newItem oldItemsCounter
            |> Option.defaultValue []
            |> List.iter (fun oldIndex ->
                let prevSubsequence =
                    match overlap.TryGetValue(oldIndex - 1) with
                    | true, value -> value
                    | false, _ -> 0

                let newSubsequence = prevSubsequence + 1
                overlap[oldIndex] <- newSubsequence

                if newSubsequence > longestSubsequence then
                    subStartOld <- oldIndex - newSubsequence + 1
                    subStartNew <- newIndex - newSubsequence + 1
                    longestSubsequence <- newSubsequence))

        { SubStartNew = subStartNew
          SubStartOld = subStartOld
          LongestSubsequence = longestSubsequence }

    [<CompiledName("GetDiffs")>]
    let rec getDiffs (oldItems: 'T list) (newItems: 'T list) : Diff<'T> list =
        match oldItems, newItems with
        | [], [] -> []
        | _, _ ->
            let { SubStartNew = subStartNew
                  SubStartOld = subStartOld
                  LongestSubsequence = longestSubsequence } =
                getLongestSubsequence oldItems newItems

            if longestSubsequence = 0 then
                [ Delete oldItems; Insert newItems ]
            else
                let oldItemsPreSubsequence = oldItems |> List.take subStartOld
                let newItemsPreSubsequence = newItems |> List.take subStartNew

                let oldItemsPostSubsequence = oldItems |> List.skip (subStartOld + longestSubsequence)
                let newItemsPostSubsequence = newItems |> List.skip (subStartNew + longestSubsequence)

                let unchangedItems = newItems |> List.skip subStartNew |> List.take longestSubsequence

                let preSubsequence = getDiffs oldItemsPreSubsequence newItemsPreSubsequence
                let postSubsequence = getDiffs oldItemsPostSubsequence newItemsPostSubsequence

                [ yield! preSubsequence; Equal unchangedItems; yield! postSubsequence ]
