using Advent2015.Solutions;
using AdventTestsBase;

namespace Advent2015Tests.Solutions;

public class Solution01Tests : AdventSolutionTests<Solution01>
{
    [Theory]
    [InlineData("(())", 0, -1)]
    [InlineData("()()", 0, -1)]
    [InlineData("(((", 3, -1)]
    [InlineData("(()(()(", 3, -1)]
    [InlineData("))(((((", 3, 0)]
    [InlineData("())", -1, 2)]
    [InlineData("))(", -1, 0)]
    [InlineData(")))", -3, 0)]
    [InlineData(")())())", -3, 0)]
    public void Test1(string line, int floor, int basementIdx)
    {
        var sut = GetSut();
        
        sut.Run([line]);

        Assert.Equal(floor, sut.Floor);
        Assert.Equal(basementIdx, sut.BasementFoundAtIndex);
    }
}