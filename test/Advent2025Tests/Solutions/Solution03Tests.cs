using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution03Tests : AdventSolutionTests<Solution03>
{
    [Theory]
    [InlineData("987654321111111", 98)]
    [InlineData("811111111111119", 89)]
    [InlineData("234234234234278", 78)]
    [InlineData("818181911112111", 92)]
    public void TestP1Examples(string line, long maxJoltage)
    {
        var solution = GetSut();

        solution.Run([line]);

        Assert.Equal(maxJoltage, solution.MaxJoltage);
    }
    
    [Theory]
    [InlineData("987654321111111", 987654321111)]
    [InlineData("811111111111119", 811111111119)]
    [InlineData("234234234234278", 434234234278)]
    [InlineData("818181911112111", 888911112111)]
    public void TestP2Examples(string bank, long maxJoltage)
    {
        var solution = GetSut();

        solution.Run([bank], true);

        Assert.Equal(maxJoltage, solution.MaxJoltage);
    }
    
    private Solution03 GetSut() => new(Logger);
}