using AdventBase;

namespace Advent2025.Solutions;

public class Solution02() : Solution("2025-02.txt", fileParseOption: SolutionParseOption.SingleLine)
{
    public long InvalidIdSum { get; private set; } = 0L;

    private readonly Dictionary<int, List<int>> _factors = GetFactors();

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        // line = 123456-198543
        foreach (var line in inputLines)
        {
            var split = line.Split('-');
            var min = long.Parse(split[0]);
            var max = long.Parse(split[1]);

            for (var i = min; i <= max; i++)
            {
                if (debug) Console.WriteLine($"Checking {i}");
                var idString = i.ToString();

                var isInvalid = partTwo ? IsInvalidPartTwo(idString, debug) : IsInvalidPartOne(idString, debug);

                if (isInvalid)
                {
                    Console.WriteLine($"Found invalid product id: {idString}");
                    InvalidIdSum += i;
                }
            }
        }

        Console.WriteLine($"Sum of invalid ids is: {InvalidIdSum}");
    }

    private bool IsInvalidPartTwo(string id, bool debug = false)
    {
        var factorsForId = _factors[id.Length];
        return IsInvalid(id, factorsForId, debug);
    }

    private static bool IsInvalidPartOne(string id, bool debug = false)
    {
        if (id.Length % 2 != 0) return false;
        return IsInvalid(id, [id.Length / 2], debug);
    }

    private static bool IsInvalid(string id, List<int> validSubstringLengths, bool debug = false)
    {
        // check smallest to largest
        List<int> lengths = [..validSubstringLengths];
        lengths.Sort((a, b) => -(a - b));

        if (debug) Console.WriteLine($"{id} => lengths to check [{string.Join(",", lengths)}]");

        foreach (var length in lengths)
        {
            if (debug) Console.WriteLine($"Checking {id} sub length=${length}");
            List<string> split = [];
            var startIdx = 0;
            var nextIdx = startIdx + length;

            while (nextIdx < id.Length)
            {
                var chunk = id[startIdx..nextIdx];
                if (debug) Console.WriteLine($"Added chunk {chunk}");
                split.Add(id[startIdx..nextIdx]);
                startIdx += length;
                nextIdx += length;
            }

            split.Add(id[startIdx..]);
            if (debug) Console.WriteLine($"Added chunk {id[startIdx..]}");

            if (new HashSet<string>(split).Count == 1) return true;
        }

        return false;
    }

    private static Dictionary<int, List<int>> GetFactors()
    {
        var result = new Dictionary<int, List<int>>();
        for (var i = 1; i <= 20; i++)
        {
            result[i] = [];
            for (var j = 1; j < i; j++)
            {
                // if i = 10,
                // we want 1, 2, 5.
                // these are valid repeated substring lengths for a string of length n.
                if (i % j == 0) result[i].Add(j);
            }
        }

        return result;
    }

    public override void Reset()
    {
        InvalidIdSum = 0L;
    }
}