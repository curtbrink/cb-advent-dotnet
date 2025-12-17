using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution09Tests : AdventSolutionTests<Solution09>
{
    [Fact]
    public void TestP1Example()
    {
        var input = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3".Split("\n").ToList();
        
        var sut = GetSut();

        sut.Run(input);
        Assert.Equal(50, sut.LargestArea);
    }
    
    [Fact]
    public void TestP2Example()
    {
        var input = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3".Split("\n").ToList();
        
        var sut = GetSut();

        sut.Run(input, true);
        Assert.Equal(24, sut.LargestValidArea);
    }
    
    private Solution09 GetSut() => new(Logger);
}