using Advent2016.Solutions;
using AdventBase;

namespace Advent2016;

public class Advent2016 : AdventYear<Advent2016>
{
    public override Solution GetSolution(string id) => id switch
    {
        "01" => new Solution01(),
        "02" => new Solution02(),
        _ => throw new ArgumentOutOfRangeException(nameof(id), "Invalid id"),
    };
}