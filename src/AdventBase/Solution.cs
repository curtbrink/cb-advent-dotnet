namespace AdventBase;

public abstract class Solution(string filename, string? filenamePartTwo = null, bool singlePart = false, SolutionParseOption fileParseOption = SolutionParseOption.MultiLine)
{
    public SolutionParseOption ParseOption { get; } = fileParseOption;
    public string FilenameP1 { get; } = filename;
    public string FilenameP2 { get; } = filenamePartTwo ?? filename;
    public bool SinglePart { get; } = singlePart;
    
    public abstract void Run(List<string> inputLines, bool partTwo = false, bool debug = false);
    public abstract void Reset();
}