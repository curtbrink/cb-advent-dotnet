using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution07Tests : AdventSolutionTests<Solution07>
{
    [Fact]
    public void TestP1Example()
    {
        var solution = GetSut();
        
        var fileString = File.ReadAllText($"TestInputs/Solution07/testinput01.txt");
        var lines = fileString.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        solution.Run(lines);

        Assert.Equal(21, solution.TotalSplits);
        Assert.Equal(9, solution.TotalBeams);
        Assert.Equal(40, solution.TotalTimelines);
    }
    
    private Solution07 GetSut() => new(Logger);
}