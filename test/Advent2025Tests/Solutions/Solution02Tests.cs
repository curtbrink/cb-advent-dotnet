using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution02Tests : AdventSolutionTests<Solution02>
{
    [Theory]
    [InlineData("11-22", 33)]
    [InlineData("95-115", 99)]
    [InlineData("998-1012", 1010)]
    [InlineData("1188511880-1188511890", 1188511885)]
    [InlineData("222220-222224", 222222)]
    [InlineData("1698522-1698528", 0)]
    [InlineData("446443-446449", 446446)]
    [InlineData("38593856-38593862", 38593859)]
    [InlineData("565653-565659", 0)]
    [InlineData("824824821-824824827", 0)]
    [InlineData("2121212118-2121212124", 0)]
    public void TestP1Examples(string ids, long expectedSum)
    {
        var sol = GetSut();
        sol.Run([ids]);

        Assert.Equal(expectedSum, sol.InvalidIdSum);
    }
    
    [Theory]
    [InlineData("11-22", 33)]
    [InlineData("95-115", 210)]
    [InlineData("998-1012", 2009)]
    [InlineData("1188511880-1188511890", 1188511885)]
    [InlineData("222220-222224", 222222)]
    [InlineData("1698522-1698528", 0)]
    [InlineData("446443-446449", 446446)]
    [InlineData("38593856-38593862", 38593859)]
    [InlineData("565653-565659", 565656)]
    [InlineData("824824821-824824827", 824824824)]
    [InlineData("2121212118-2121212124", 2121212121)]
    public void TestP2Examples(string ids, long expectedSum)
    {
        var sol = GetSut();
        sol.Run([ids], true);

        Assert.Equal(expectedSum, sol.InvalidIdSum);
    }
    
    private Solution02 GetSut() => new(Logger);
}