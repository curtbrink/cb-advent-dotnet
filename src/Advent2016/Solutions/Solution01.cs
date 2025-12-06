using AdventBase;
using AdventBase.Utils;

namespace Advent2016.Solutions;

public class Solution01() : Solution("2016-01.txt", fileParseOption: SolutionParseOption.SingleLine)
{
    public int TotalDistance => Math.Abs(_x) + Math.Abs(_y);

    private int _x = 0;
    private int _y = 0;
    private Direction _direction = Direction.North;
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        if (partTwo) return;

        foreach (var line in inputLines)
        {
            var turnAction = line[0];
            var distance = int.Parse(line[1..]);

            _direction = turnAction == 'L' ? _direction.TurnLeft() : _direction.TurnRight();
            
            var sign = _direction.IsNegativeOnAxis() ? -1 : 1;
            if (_direction.IsXAxis())
            {
                _x += sign * distance;
            }
            else
            {
                _y += sign * distance;
            }
        }

        Console.WriteLine($"I am {TotalDistance} units away from spawn!");
    }

    public override void Reset()
    {
        _x = 0;
        _y = 0;
        _direction = Direction.North;
    }
}