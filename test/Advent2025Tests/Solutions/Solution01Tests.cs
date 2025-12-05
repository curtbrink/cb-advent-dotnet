using Advent2025.Solutions;

namespace AdventTests.Solutions;

public class Solution01Tests
{
    [Fact]
    public void Solution01TestZeroes()
    {
        // this "dial program" hits every type of way to cross a zero:
        // - moving left or right with any combination of one or more:
        //     - starting on zero
        //     - ending on zero
        //     - crossing no zeroes
        //     - crossing exactly 1 zero
        //     - crossing multiple zeroes
        List<string> basicLines =
        [
            "L50", // 0 1+ 2+
            "R50", // 50
            "R100", // 150 2+
            "R50", // 200 1+ 2+
            "R200", // 400 1+ 2+2
            "L150", // 250 2+
            "L50", // 200 1+ 2+
            "L200", // 0 1+ 2+2
            "L100", // -100 1+ 2+
            "R50", // -50
            "L100", // -150 2+
            "L200", // -350 2+2
            "R50", // -300 1+ 2+
            "R300", // 0 1+ 2+3
        ];
        var solution = GetSut();

        solution.Run(basicLines, false, true);
        var actualP1 = solution.ZeroCount;
        Assert.Equal(8, actualP1);

        solution.Reset();
        solution.Run(basicLines, true, true);
        var actualP2 = solution.ZeroCount;
        Assert.Equal(17, actualP2);
    }

    private static Solution01 GetSut() => new();
}