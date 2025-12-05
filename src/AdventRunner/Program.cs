var year = new Advent2025.Advent2025();
var day = "04";

var solution = year.GetSolution(day);
var separator = year.IsOneLine(day) ? ',' : '\n';
var lines = year.GetFileContents(day).Split(separator).Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l))
    .ToList();

Console.WriteLine("=== part one ===");
solution.Run(lines);
solution.Reset();
Console.WriteLine("=== part two ===");
solution.Run(lines, true);
