using Advent2025.Solutions;
using AdventBase;

namespace Advent2025;

public class Advent2025 : AdventYear<Advent2025>
{
    public override Solution GetSolution(string id) => id switch
    {
        "01" => new Solution01(),
        "02" => new Solution02(),
        "03" => new Solution03(),
        "04" => new Solution04(),
        "05" => new Solution05(),
        "06" => new Solution06(),
        _ => throw new Exception($"Solution not found for day \"{id}\""),
    };

    public override bool IsOneLine(string id) => id switch
    {
        "02" => true,
        _ => false,
    };

    public override bool ShouldTrimLines(string id) => id switch
    {
        "06" => false,
        _ => true,
    };

    public override string GetFileContents(string id) => File.ReadAllText($"Inputs/{id}.txt");
}