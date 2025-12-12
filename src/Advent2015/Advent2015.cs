using Advent2015.Solutions;
using AdventBase;

namespace Advent2015;

public class Advent2015 : AdventYear<Advent2015>
{
    public override Solution GetSolution(string id) => id switch
    {
        "01" => new Solution01(),
        "02" => new Solution02(),
        "03" => new Solution03(),
        "04" => new Solution04(),
        "05" => new Solution05(),
        "06" => new Solution06(),
        _ => throw new NotImplementedException(),
    };
}