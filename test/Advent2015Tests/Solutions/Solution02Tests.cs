using Advent2015.Solutions;
using AdventTestsBase;

namespace Advent2015Tests.Solutions;

public class Solution02Tests : AdventSolutionTests<Solution02>
{
    [Theory]
    [InlineData("2x3x4", 58, 34)]
    [InlineData("1x1x10", 43, 14)]
    public void TestP1P2Examples(string present, int expectedPaper, int expectedRibbon)
    {
        var sut = GetSut();

        sut.Run([present]);
        
        Assert.Equal(expectedPaper, sut.Paper);
        Assert.Equal(expectedRibbon, sut.Ribbon);
    }
}