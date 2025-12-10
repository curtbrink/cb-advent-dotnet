using AdventBase;
using AdventBase.Utils;

namespace Advent2025.Solutions;

public class Solution09() : Solution("2025-09.txt", singlePart: true)
{
    public long LargestArea { get; private set; } = 0L;
    public long LargestValidArea { get; private set; } = 0L;
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        var allTiles = (from line in inputLines
            where !string.IsNullOrEmpty(line)
            select line.Split(',').Select(long.Parse).ToList()
            into coords
            select new Tile(coords[0], coords[1])).ToList();

        var allSegments = allTiles.Select((t, i) => new Rectangle(t, allTiles[i == allTiles.Count - 1 ? 0 : i + 1]))
            .ToList();
        
        // this feels like a very very very similar problem to yesterday's...so same approach? at least for now?
        // you're right part two is much more of a doozy
        // but also I drank way too much koolaid before coming across AABB collision.
        // ...yikes. this file used to be like 450 lines of line segments and corner math and other such garbage.
        
        // max heap priority queue, because we want the largest area...
        var areaQueue =
            new PriorityQueue<Rectangle, long>(Comparer<long>.Create((l, l1) => l1.CompareTo(l)));

        var allPairs = new HashSet<BidirectionalTuple<Tile>>();
        foreach (var tile in allTiles)
        {
            foreach (var tile2 in allTiles.Where(t => t != tile))
            {
                var tuple = new BidirectionalTuple<Tile>(tile, tile2);
                if (!allPairs.Add(tuple)) continue;

                var rect = new Rectangle(tile, tile2);
                areaQueue.Enqueue(rect, rect.Area);
            }
        }
        
        // now just get the largest rectangle for p1
        var largest = areaQueue.Peek();
        LargestArea = largest.Area;
        Console.WriteLine($"(part one) The largest area possible is {LargestArea}");
        
        // part two - make sure they're valid

        Rectangle? largestValid = null;
        while (largestValid == null && areaQueue.Count > 0)
        {
            var largestToCheck = areaQueue.Dequeue();

            if (allSegments.All(segment => !largestToCheck.Intersects(segment)))
            {
                largestValid = largestToCheck;
            }
        }

        LargestValidArea = largestValid!.Area;
        Console.WriteLine($"(part two) Largest valid rectangle is {largestValid.Area}");
    }

    public override void Reset()
    {
    }
    
    private record Tile(long X, long Y);

    private record Rectangle(Tile A, Tile B)
    {
        public long Area => (Math.Abs(A.X - B.X) + 1) * (Math.Abs(A.Y - B.Y) + 1);
        
        public bool Intersects(Rectangle other)
        {
            var aLeftOfB = Math.Max(A.X, B.X) <= Math.Min(other.A.X, other.B.X);
            var aRightOfB = Math.Min(A.X, B.X) >= Math.Max(other.A.X, other.B.X);
            var aAboveB = Math.Max(A.Y, B.Y) <= Math.Min(other.A.Y, other.B.Y);
            var aBelowB = Math.Min(A.Y, B.Y) >= Math.Max(other.A.Y, other.B.Y);

            return !(aLeftOfB || aRightOfB || aAboveB || aBelowB);
        }
    }
}
