using Advent2016.Solutions;
using AdventTestsBase;

namespace Advent2016Tests.Solutions;

public class Solution01Tests : AdventSolutionTests<Solution01>
{
    [Theory]
    [InlineData("L2, R3", 5)]
    [InlineData("R2, R2, R2", 2)]
    [InlineData("R5, L5, R5, R3", 12)]
    public void TestP1Examples(string directions, int expected)
    {
        var lines = directions.Split(',').Select(l => l.Trim()).ToList();
        var solution = GetSut();
        
        solution.Run(lines);
        
        Assert.Equal(expected, solution.TotalDistance);
    }

    [Fact]
    public void TestP2Example()
    {
        List<string> lines = "R8, R4, R4, R8".Split(',').Select(l => l.Trim()).ToList();
        var solution = GetSut();

        solution.Run(lines, true);

        Assert.Equal(4, solution.TotalDistance);
    }
}