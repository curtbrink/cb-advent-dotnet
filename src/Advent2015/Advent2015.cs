using Advent2015.Solutions;
using AdventBase;

namespace Advent2015;

public class Advent2015 : AdventYear<Advent2015>
{
    public override Solution GetSolution(string id) => id switch
    {
        "01" => new Solution01(),
        _ => throw new NotImplementedException(),
    };
}