using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution06Tests : AdventSolutionTests<Solution06>
{
    [Fact]
    public void TestP1Example()
    {
        var input = """
                    123 328  51 64 
                     45 64  387 23 
                      6 98  215 314
                    *   +   *   +  
                    """.Split('\n').ToList();

        var sut = GetSut();

        sut.Run(input);

        Assert.Equal(4277556, sut.Sum);
    }
    
    [Fact]
    public void TestP2Example()
    {
        var input = """
                    123 328  51 64 
                     45 64  387 23 
                      6 98  215 314
                    *   +   *   +  
                    """.Split('\n').ToList();

        var sut = GetSut();

        sut.Run(input, true);

        Assert.Equal(3263827, sut.Sum);
    }
    
    private Solution06 GetSut() => new(Logger);
}