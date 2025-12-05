using AdventBase;

// ======= SET THESE
var year = 2016;
var id = "01";

// ======= DON'T CHANGE BELOW THIS LINE

var years = new Dictionary<int, AdventYear>
{
    [2016] = new Advent2016.Advent2016(),
    [2025] = new Advent2025.Advent2025(),
};

var y = years[year];
var solution = y.GetSolution(id);
var separator = y.IsOneLine(id) ? ',' : '\n';
var lines = y.GetFileContents(id).Split(separator).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToList();

Console.WriteLine("=== part one ===");
solution.Run(lines);
solution.Reset();
Console.WriteLine("=== part two ===");
solution.Run(lines, true);
