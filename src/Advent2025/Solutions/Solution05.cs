using AdventBase;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution05(ILogger<Solution05> logger) : Solution(2025, "05", "2025-05.txt")
{
    public int NumberOfFresh { get; private set; }
    
    public long TotalPossibleFresh { get; private set; }

    // keep this for part two
    private List<FreshRange> _freshRanges = [];
    
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        if (partTwo) PartTwo(inputLines);
        else PartOne(inputLines);
    }

    private void PartOne(List<string> inputLines)
    {
        var idsToCheck = new List<string>();
        // parse ranges
        var isParsingRanges = true;
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                isParsingRanges = false;
                continue;
            }

            if (isParsingRanges)
            {
                // line is "12345-67890"
                var split = line.Split('-');
                var range = new FreshRange(split[0], split[1], logger);
                _freshRanges.Add(range);
            }
            else
            {
                idsToCheck.Add(line);
            }
        }

        NumberOfFresh = idsToCheck.Count(id => _freshRanges.Any(range => range.IsFresh(id)));
        logger.LogInformation("There are {NumberOfFresh} fresh ingredient ids!", NumberOfFresh);
    }

    private void PartTwo(List<string> inputLines)
    {
        // same as before, but we need to dedupe!
        // parse ranges
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            // line is "12345-67890"
            var split = line.Split('-');

            _freshRanges.Add(new FreshRange(split[0], split[1], logger));
        }

        var wasReduced = true;
        while (wasReduced)
        {
            wasReduced = ReduceRanges();
        }
        
        TotalPossibleFresh = _freshRanges.Sum(range => range.TotalPossible());
        logger.LogInformation("There are {TotalPossibleFresh} possible fresh ingredient ids!", TotalPossibleFresh);
    }

    private bool ReduceRanges()
    {
        // dedupe ranges repeatedly
        var listCopy = _freshRanges.ToList();
        _freshRanges = [];
        
        var wasReduced = false;

        for (var i = 0; i < listCopy.Count; i++)
        {
            var checkingRange = listCopy[i];
            var wasCombined = false;
            for (var j = i + 1; j < listCopy.Count; j++)
            {
                var againstRange = listCopy[j];

                var isExclusiveRanges =
                    checkingRange.Start > againstRange.End || checkingRange.End < againstRange.Start;

                if (isExclusiveRanges) continue;
                
                // merge, remove from list
                var newRange = checkingRange.Extend(againstRange.Start.ToString(), againstRange.End.ToString());
                _freshRanges.Add(newRange);
                wasReduced = true;
                wasCombined = true;
                listCopy.RemoveAt(j);
                listCopy.RemoveAt(i);
                i--;
                break;
            }

            if (!wasCombined)
            {
                _freshRanges.Add(checkingRange);
            }
            
            // if overlaps with another range, combine them and add to freshranges.
            // otherwise add it to freshranges outright.
        }

        return wasReduced;
    }

    public override void Reset()
    {
        NumberOfFresh = 0;
        TotalPossibleFresh = 0;
        _freshRanges = [];
    }
}

public class FreshRange(string start, string end, ILogger<Solution05> logger)
{
    public readonly long Start = long.Parse(start);
    public readonly long End = long.Parse(end);

    public bool IsFresh(string id)
    {
        var i = long.Parse(id);
        return i >= Start && i <= End;
    }

    public long TotalPossible() => End - Start + 1;

    public FreshRange Extend(string start, string end)
    {
        logger.LogDebug("Extended! before: {Start} - {End}", Start, End);
        var newStart = Math.Min(Start, long.Parse(start));
        var newEnd = Math.Max(End, long.Parse(end));
        logger.LogDebug("after: {Start} - {End}", newStart, newEnd);
        return new FreshRange(newStart.ToString(), newEnd.ToString(), logger);
    }
}