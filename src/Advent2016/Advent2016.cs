using Advent2016.Solutions;
using AdventBase;

namespace Advent2016;

public class Advent2016 : AdventYear<Advent2016>
{
    public override Solution GetSolution(string id) => id switch
    {
        "01" => new Solution01(),
        _ => throw new ArgumentOutOfRangeException(nameof(id), "Invalid id"),
    };

    public override bool IsOneLine(string id) => id switch
    {
        "01" => true,
        _ => false,
    };

    public override string GetFileContents(string id) => File.ReadAllText($"Inputs/2016-{id}.txt");
}