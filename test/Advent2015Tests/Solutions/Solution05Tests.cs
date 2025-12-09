using Advent2015.Solutions;
using AdventTestsBase;

namespace Advent2015Tests.Solutions;

public class Solution05Tests : AdventSolutionTests<Solution05>
{
    [Theory]
    [InlineData("ugknbfddgicrmopn", true)]
    [InlineData("aaa", true)]
    [InlineData("jchzalrnumimnmhp", false)]
    [InlineData("haegwjzuvuyypxyu", false)]
    [InlineData("dvszwmarrgswjxmb", false)]
    public void TestP1Examples(string input, bool isNice)
    {
        var sut = GetSut();
        
        sut.Run([input]);

        Assert.Equal(isNice ? 1 : 0, sut.NumberOfNice);
    }
    
    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb", true)]
    [InlineData("xxyxx", true)]
    [InlineData("uurcxstgmygtbstg", false)]
    [InlineData("ieodomkazucvgmuy", false)]
    public void TestP2Examples(string input, bool isNice)
    {
        var sut = GetSut();
        
        sut.Run([input], true);

        Assert.Equal(isNice ? 1 : 0, sut.NumberOfNice);
    }
}