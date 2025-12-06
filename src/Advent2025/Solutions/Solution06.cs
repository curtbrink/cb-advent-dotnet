using System.Text.RegularExpressions;
using AdventBase;
using AdventBase.Utils;

namespace Advent2025.Solutions;

public partial class Solution06 : Solution
{
    public long Sum { get; private set; } = 0L;
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        if (partTwo) TransposeAndProcess(inputLines);
        else JustProcess(inputLines);
    }

    private void JustProcess(List<string> inputLines)
    {
        // part one
        var problems = new List<List<string>>();
        var isFirstLine = true;
        foreach (var line in inputLines)
        {
            var parts = line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            for (var i = 0; i < parts.Count; i++)
            {
                if (isFirstLine)
                {
                    problems.Add([]);
                }
                problems[i].Add(parts[i]);
            }

            isFirstLine = false;
        }

        foreach (var problem in problems)
        {
            var operat = problem[^1];
            var shouldMultiply = operat == "*";

            var operands = problem[0..^1];
            long total = shouldMultiply ? 1L : 0L;
            foreach (var operand in operands)
            {
                var p = long.Parse(operand);
                if (shouldMultiply) total *= p;
                else total += p;
            }

            Sum += total;
        }

        Console.WriteLine($"Sum of all problems: {Sum}");
    }

    private void TransposeAndProcess(List<string> inputLines)
    {
        // part two
        
        // helper grid does a lot of heavy lifting :3
        var transposed = new AdventGrid(inputLines, true).RotateLeft();

        // now we just run down the list
        var currentNums = new List<long>();
        long total = 0L;
        foreach (var line in transposed.Select(l => l.Trim()))
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (IsPureNumber().IsMatch(line))
            {
                var num = long.Parse(line);
                currentNums.Add(num);
                continue;
            }
            // else we have a sign at the end, need to calculate!
            if (line[^1] != '+' && line[^1] != '*')
            {
                throw new Exception($"Unknown syntax: {line}");
            }

            // grab the last number first
            var lastNumber = long.Parse(line[0..^1]);
            currentNums.Add(lastNumber);
            
            // calculate
            var shouldMultiply = line[^1] == '*';
            Func<long,long,long> accumulator = shouldMultiply
                ? (currentSum, next) => currentSum *= next
                : (currentSum, next) => currentSum += next;
            var seed = shouldMultiply ? 1L : 0L;

            total += currentNums.Aggregate(seed, accumulator);
            
            // wipe for next problem
            currentNums.Clear();
        }

        Sum = total;
        Console.WriteLine($"Sum of all problems: {Sum}");
    }

    public override void Reset()
    {
        Sum = 0L;
    }
    
    [GeneratedRegex("^\\d+$")]
    private static partial Regex IsPureNumber();
}