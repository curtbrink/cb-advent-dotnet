namespace AdventBase.Utils;

public class AdventGrid
{
    private readonly char[][] _grid;
    public int Width { get; }
    public int Height { get; }

    public AdventGrid(IList<string> lines, bool pad = false)
    {
        if (lines.Count == 0 || string.IsNullOrEmpty(lines[0]))
            throw new ArgumentOutOfRangeException(nameof(lines), "Doesn't look like a grid to me...");

        Width = lines.Max(x => x.Length);
        Height = lines.Count;

        _grid = new char[Height][];
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Length != Width && !pad)
                throw new ArgumentOutOfRangeException(nameof(lines), "Lines have inconsistent size...");
            else
                line = line.PadRight(Width);
            
            _grid[i] = new char[Width];
            for (var j = 0; j < Width; j++)
            {
                _grid[i][j] = line[j];
            }
        }
    }

    public char GetCell(int x, int y) => _grid[y][x];
    
    protected char SetCell(int x, int y, char value) => _grid[y][x] = value;

    public bool CellIs(int x, int y, char n) => _grid[y][x] == n;

    protected IList<(int x, int y)> GetNeighbors(int x, int y) => new List<(int x, int y)>
    {
        (x - 1, y - 1),
        (x, y - 1),
        (x + 1, y - 1),
        (x - 1, y),
        (x + 1, y),
        (x - 1, y + 1),
        (x, y + 1),
        (x + 1, y + 1),
    }.Where(xy => xy.x >= 0 && xy.x < Width && xy.y >= 0 && xy.y < Height).ToList();
}

public static class AdventGridExtensions
{
    extension(AdventGrid grid)
    {
        public List<string> RotateLeft()
        {
            // construct new grid
            // [[0, 1, 2],
            //  [3, 4, 5]]
            // becomes
            // [[2, 5],
            //  [1, 4],
            //  [0, 3]]

            var newLines = new List<string>();
            
            // iterate columns first
            for (var x = 0; x < grid.Width; x++)
            {
                // right to left, though
                var gridIdxX = grid.Width - 1 - x;
                newLines.Add("");
                for (var y = 0; y < grid.Height; y++)
                {
                    newLines[x] += grid.GetCell(gridIdxX, y);
                }
            }

            return newLines;
        }
    }
}