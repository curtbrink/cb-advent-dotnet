using AdventBase;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution02(ILogger<Solution02> logger) : Solution(2025, "02", "2025-02.txt", fileParseOption: SolutionParseOption.SingleLine)
{
    public long InvalidIdSum { get; private set; } = 0L;

    private readonly Dictionary<int, List<int>> _factors = GetFactors();

    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        // line = 123456-198543
        foreach (var line in inputLines)
        {
            var split = line.Split('-');
            var min = long.Parse(split[0]);
            var max = long.Parse(split[1]);

            for (var i = min; i <= max; i++)
            {
                logger.LogDebug("Checking {i}", i);
                var idString = i.ToString();

                var isInvalid = partTwo ? IsInvalidPartTwo(idString) : IsInvalidPartOne(idString);

                if (isInvalid)
                {
                    logger.LogInformation("Found invalid product id: {idString}", idString);
                    InvalidIdSum += i;
                }
            }
        }

        logger.LogInformation("Sum of invalid ids is: {InvalidIdSum}", InvalidIdSum);
    }

    private bool IsInvalidPartTwo(string id)
    {
        var factorsForId = _factors[id.Length];
        return IsInvalid(id, factorsForId);
    }

    private bool IsInvalidPartOne(string id)
    {
        if (id.Length % 2 != 0) return false;
        return IsInvalid(id, [id.Length / 2]);
    }

    private bool IsInvalid(string id, List<int> validSubstringLengths)
    {
        // check smallest to largest
        List<int> lengths = [..validSubstringLengths];
        lengths.Sort((a, b) => -(a - b));

        logger.LogDebug("{id} => lengths to check [{lengths}]", id, string.Join(",", lengths));

        foreach (var length in lengths)
        {
            logger.LogDebug("Checking {id} sub length=${length}", id, length);
            List<string> split = [];
            var startIdx = 0;
            var nextIdx = startIdx + length;

            while (nextIdx < id.Length)
            {
                var chunk = id[startIdx..nextIdx];
                logger.LogDebug("Added chunk {chunk}", chunk);
                split.Add(id[startIdx..nextIdx]);
                startIdx += length;
                nextIdx += length;
            }

            split.Add(id[startIdx..]);
            logger.LogDebug("Added chunk {chunk}", id[startIdx..]);

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