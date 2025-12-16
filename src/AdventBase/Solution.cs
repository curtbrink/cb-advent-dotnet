namespace AdventBase;

public abstract class Solution(int year, string id, string filename, string? filenamePartTwo = null, bool singlePart = false, SolutionParseOption fileParseOption = SolutionParseOption.MultiLine)
{
    internal int Year { get; } = year;
    internal string Id { get; } = id;
    public SolutionParseOption ParseOption { get; } = fileParseOption;
    public string FilenameP1 { get; } = filename;
    public string FilenameP2 { get; } = filenamePartTwo ?? filename;
    public bool SinglePart { get; } = singlePart;
    
    public abstract void Run(List<string> inputLines, bool partTwo = false);
    public abstract void Reset();
}