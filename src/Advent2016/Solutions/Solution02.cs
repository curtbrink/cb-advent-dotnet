using AdventBase;
using AdventBase.Utils;
using Microsoft.Extensions.Logging;

namespace Advent2016.Solutions;

public class Solution02(ILogger<Solution02> logger) : Solution(2016, "02", "2016-02.txt")
{
    public string KeyCode { get; private set; } = "";

    private char[][] _keyPadP1 = [['1', '2', '3'], ['4', '5', '6'], ['7', '8', '9']];

    private char[][] _keyPadP2 =
    [
        [' ', ' ', '1', ' ', ' '], [' ', '2', '3', '4', ' '], ['5', '6', '7', '8', '9'], [' ', 'A', 'B', 'C', ' '],
        [' ', ' ', 'D', ' ', ' ']
    ];
    private int _x = 1;
    private int _y = 1;
    
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            
            foreach (var vec in line.Select(dir => Direction.From(dir).Vector()))
            {
                if (partTwo) MoveAndClampP2(vec.x, vec.y);
                else MoveAndClampP1(vec.x, vec.y);
            }

            Next(partTwo ? _keyPadP2 : _keyPadP1);
        }

        logger.LogInformation("My keycode is {KeyCode}", KeyCode);
    }

    private void MoveAndClampP1(int x, int y)
    {
        _x += x;
        _y += y;
        if (_x < 0) _x = 0;
        if (_x > 2) _x = 2;
        if (_y < 0) _y = 0;
        if (_y > 2) _y = 2;
    }

    private void MoveAndClampP2(int x, int y)
    {
        var newX = _x + x;
        var newY = _y + y;
        logger.LogDebug("Attempting move from {_x},{_y} to {newX},{newY}", _x, _y, newX, newY);
        if (newX < 0 || newX > 4 || newY < 0 || newY > 4 || _keyPadP2[newY][newX] == ' ') return;

        _x = newX;
        _y = newY;
    }

    private void Next(char[][] pad)
    {
        var boop = pad[_y][_x];
        KeyCode += boop;
    }

    public override void Reset()
    {
        // set this to P2 starting pos
        KeyCode = "";
        _x = 0;
        _y = 2;
    }
}