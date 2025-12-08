using Advent2015.Solutions;
using AdventTestsBase;

namespace Advent2015Tests.Solutions;

public class Solution03Tests : AdventSolutionTests<Solution03>
{
    [Theory]
    [InlineData(">", 2, 2)]
    [InlineData("^v", 2, 3)]
    [InlineData("^>v<", 4, 3)]
    [InlineData("^v^v^v^v^v", 2, 11)]
    public void TestP1P2Examples(string instructions, int expectedP1, int expectedP2)
    {
        var sut = GetSut();

        // part one
        sut.Run([instructions]);
        
        Assert.Equal(expectedP1, sut.HousesVisited);
        
        // part two
        sut.Reset();
        sut.Run([instructions], true);
        Assert.Equal(expectedP2, sut.HousesVisited);
    }
}