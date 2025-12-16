using AdventBase;
using AdventBase.Utils;

namespace Advent2025.Solutions;

public class Solution04() : Solution(2025, "04", "2025-04.txt")
{
    public long PaperRolls { get; private set; } = 0;
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        var grid = new PaperRollGrid(inputLines, '.', '@');

        PaperRolls = partTwo ? grid.CountRemovedPaperRolls() : grid.GetAccessiblePaperRolls().Count;

        Console.WriteLine($"Count of accessible rolls: {PaperRolls}");
    }

    public override void Reset()
    {
        PaperRolls = 0;
    }
}

public class PaperRollGrid(IList<string> lines, char floor, char paper) : AdventGrid(lines)
{
    public List<(int x, int y)> GetAccessiblePaperRolls()
    {
        // accessible, adj. -> a floor tile that is both:
        //                       (1) occupied by a paper roll, and
        //                       (2) surrounded by no more than 3 other paper rolls
        var accessibleRolls = new List<(int x, int y)>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (!CellIs(x, y, paper)) continue;
                if (GetNeighbors(x, y).Count(tuple => GetCell(tuple.x, tuple.y) == paper) > 3) continue;
                accessibleRolls.Add((x, y));
            }
        }

        return accessibleRolls;
    }

    public long CountRemovedPaperRolls()
    {
        var countOfRemovedPaperRolls = 0;

        var removedSomeRolls = true;
        while (removedSomeRolls)
        {
            removedSomeRolls = false;

            var markedForRemoval = GetAccessiblePaperRolls();
            if (markedForRemoval.Count > 0)
            {
                removedSomeRolls = true;
                countOfRemovedPaperRolls += markedForRemoval.Count;
            }
            
            foreach (var tuple in markedForRemoval)
            {
                SetCell(tuple.x, tuple.y, floor);
            }
        }

        return countOfRemovedPaperRolls;
    }
}
