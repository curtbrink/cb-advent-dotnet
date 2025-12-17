using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution05Tests : AdventSolutionTests<Solution05>
{
    [Fact]
    public void TestPartTwo()
    {
        List<string> lines = ["3-5", "10-14", "16-20", "12-18", ""];

        var sol = GetSut();

        sol.Run(lines, true);

        Assert.Equal(14, sol.TotalPossibleFresh);
    }
    
    private Solution05 GetSut() => new(Logger);
}