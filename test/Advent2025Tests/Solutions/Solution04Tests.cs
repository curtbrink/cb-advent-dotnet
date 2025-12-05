using Advent2025.Solutions;

namespace AdventTests.Solutions;

public class Solution04Tests
{
    [Theory]
    [InlineData("testinput01.txt", 13)]
    public void TestP1Examples(string file, long paperRolls)
    {
        var solution = GetSut();
        
        var fileString = File.ReadAllText($"TestInputs/Solution04/{file}");
        var lines = fileString.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        solution.Run(lines);

        Assert.Equal(paperRolls, solution.PaperRolls);
    }
    
    [Theory]
    [InlineData("testinput01.txt", 43)]
    public void TestP2Examples(string file, long paperRolls)
    {
        var solution = GetSut();
        
        var fileString = File.ReadAllText($"TestInputs/Solution04/{file}");
        var lines = fileString.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        solution.Run(lines, true);

        Assert.Equal(paperRolls, solution.PaperRolls);
    }
    
    private Solution04 GetSut() => new();
}