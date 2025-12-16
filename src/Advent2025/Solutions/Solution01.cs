using AdventBase;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution01(ILogger<Solution01> logger) : Solution(2025, "01", "2025-01.txt")
{
    public long Dial { get; private set; } = 50L;
    public long ZeroCount { get; private set; } = 0L;
    public long LastHundred { get; private set; } = 0L;
    public bool LastEndedOnZero { get; private set; } = false;

    public override void Reset()
    {
        Dial = 50L;
        ZeroCount = 0L;
        LastHundred = 0L;
        LastEndedOnZero = false;
    }

    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        foreach (var line in inputLines)
        {
            var lastZeroCount = ZeroCount;
            var direction = line[0..1];
            var amount = int.Parse(line[1..]);
            
            var sign = direction == "R" ? 1 : -1;
            var toMove = sign * amount;
            
            Dial += toMove;

            // going from 50 -> -50 should go from 0 -> -1 hundred, so if dial is negative, subtract 1.
            var remainder = Dial % 100;
            var wholeHundred = Dial / 100;
            var newHundred = Dial < 0 && remainder != 0L ? wholeHundred - 1 : wholeHundred;

            switch (partTwo)
            {
                case false when remainder == 0L:
                    ZeroCount++;
                    break;
                case true:
                {
                    // moving right from 0 -> 50 = 0
                    // moving right from 0 -> 100 = 1
                    // moving right from 0 -> 150 = 1
                    // moving right from 0 -> 200 = 2
                    // moving right from 1 -> 50 = 0
                    // moving right from 1 -> 100 = 1
                    // moving right from 1 -> 150 = 1
                    // moving right from 1 -> 200 = 2
                    // moving left from 200 -> 150 = 0
                    // moving left from 200 -> 100 = 1
                    // moving left from 200 -> 50 = 1
                    // moving left from 200 -> 0 = 2
                    // moving left from 201 -> 150 = 1
                    // moving left from 201 -> 100 = 2
                    // moving left from 201 -> 50 = 2
                    // moving left from 201 -> 0 = 3
                    if (newHundred != LastHundred || remainder == 0L)
                    {
                        var diff = Math.Abs(newHundred - LastHundred);
                        if (sign == -1 && remainder == 0L) diff++;
                        if (sign == -1 && LastEndedOnZero) diff--;
                        ZeroCount += diff;
                    }
                    break;
                }
            }

            if (lastZeroCount != ZeroCount)
                logger.LogDebug(
                    "[debug] moved={toMove} newD={Dial} lastH={LastHundred} newH={newHundred} zeroCount={ZeroCount}",
                    toMove, Dial, LastHundred, newHundred, ZeroCount);

            LastHundred = newHundred;
            LastEndedOnZero = remainder == 0L;
        }

        logger.LogInformation("Number of rotations that brought dial to exactly 0: {ZeroCount}", ZeroCount);
    }
}