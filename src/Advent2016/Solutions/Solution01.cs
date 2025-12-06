using AdventBase;
using AdventBase.Utils;

namespace Advent2016.Solutions;

public class Solution01() : Solution("2016-01.txt", fileParseOption: SolutionParseOption.SingleLine)
{
    public int TotalDistance => Math.Abs(_x) + Math.Abs(_y);

    private int _x = 0;
    private int _y = 0;
    private Direction _direction = Direction.North;
    private HashSet<ValueTuple<int, int>> _haveVisited = [ValueTuple.Create(0, 0)];
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        var haveVisited = new HashSet<ValueTuple<int, int>>();
        haveVisited.Add(ValueTuple.Create(_x, _y));
        Console.WriteLine("Added 0,0 to haveVisited");

        foreach (var line in inputLines)
        {
            var turnAction = line[0];
            var distance = int.Parse(line[1..]);

            _direction = turnAction == 'L' ? _direction.TurnLeft() : _direction.TurnRight();

            if (!partTwo)
            {
                MoveP1(distance);
            }
            else
            {
                if (MoveP2(distance)) break;
            }
        }

        Console.WriteLine($"I am {TotalDistance} units away from spawn!");
    }

    private void MoveP1(int distance)
    {
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

    private bool MoveP2(int distance)
    {
        // move along the line and check if we've been there.
        var amount = _direction.IsNegativeOnAxis() ? -1 : 1;
        var intersected = false;
        for (var i = 0; i < distance; i++)
        {
            if (_direction.IsXAxis()) _x += amount;
            else _y += amount;

            if (!_haveVisited.Add(ValueTuple.Create(_x, _y)))
            {
                intersected = true;
                break;
            }
        }

        return intersected;
    }

    public override void Reset()
    {
        _x = 0;
        _y = 0;
        _direction = Direction.North;
        
    }
}