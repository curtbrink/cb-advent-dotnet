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
        "07" => new Solution07(),
        "08" => new Solution08(),
        "09" => new Solution09(),
        "10a" => new Solution10(),
        "10b" => new Solution10Z3(),
        _ => throw new Exception($"Solution not found for day \"{id}\""),
    };
}