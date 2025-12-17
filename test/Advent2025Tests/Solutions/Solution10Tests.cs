using Advent2025.Solutions;
using AdventTestsBase;

namespace AdventTests.Solutions;

public class Solution10Tests : AdventSolutionTests<Solution10>
{
    [Theory]
    [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 2u)]
    [InlineData("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 3u)]
    [InlineData("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 2u)]
    public void TestP1Example(string input, int expectedPresses)
    {
        var sol = GetSut();
        
        sol.Run([input]);

        Assert.Equal(expectedPresses, sol.MinimumPresses);
    }
    
    [Theory]
    [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 10)]
    [InlineData("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 12)]
    [InlineData("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 11)]
    public void TestP2Example(string input, int expectedPresses)
    {
        var sol = GetSut();
        
        sol.Run([input], true);

        Assert.Equal(expectedPresses, sol.MinimumPresses);
    }
    
    private Solution10 GetSut() => new(Logger);
}