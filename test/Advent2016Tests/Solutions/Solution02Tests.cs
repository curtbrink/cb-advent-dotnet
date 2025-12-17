using Advent2016.Solutions;
using AdventTestsBase;

namespace Advent2016Tests.Solutions;

public class Solution02Tests : AdventSolutionTests<Solution02>
{
    private readonly List<string> _lines = ["ULL", "RRDDD", "LURDL", "UUUUD"];
    
    [Fact]
    public void TestP1Example()
    {
        var sut = GetSut();

        sut.Run(_lines);
        
        Assert.Equal("1985", sut.KeyCode);
    }
    
    [Fact]
    public void TestP2Example()
    {
        var sut = GetSut();
        sut.Reset(); // need P2 starting pos

        sut.Run(_lines, true);
        
        Assert.Equal("5DB3", sut.KeyCode);
    }
    
    private Solution02 GetSut() => new(Logger);
}