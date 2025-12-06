using AdventBase;

// ======= SET THESE
var year = 2015;
var id = "01";

// ======= DON'T CHANGE BELOW THIS LINE

var years = new Dictionary<int, AdventYear>
{
    [2015] = new Advent2015.Advent2015(),
    [2016] = new Advent2016.Advent2016(),
    [2025] = new Advent2025.Advent2025(),
};

// look up solution
var y = years[year];
var solution = y.GetSolution(id);

// configure input parsing
var separator = solution.ParseOption switch
{
    SolutionParseOption.MultiLine or SolutionParseOption.MultiLineNoTrim => '\n',
    SolutionParseOption.SingleLine or SolutionParseOption.SingleLineNoTrim => ',',
    _ => throw new ArgumentOutOfRangeException(nameof(solution.ParseOption), "Invalid parse option"),
};

Func<string,string> trimFunc = solution.ParseOption switch
{
    SolutionParseOption.MultiLine or SolutionParseOption.SingleLine => (string s) => s.Trim(),
    SolutionParseOption.MultiLineNoTrim or SolutionParseOption.SingleLineNoTrim => (string s) => s,
    _ => throw new ArgumentOutOfRangeException(nameof(solution.ParseOption), "Invalid parse option"),
};

// idc if I'm overengineering the crap out of this runner ;3
var parsePipeline = (string filename) => File.ReadAllText(filename).Split(separator).Select(trimFunc).ToList();

// grab file contents
var filepathP1 = $"Inputs/{solution.FilenameP1}";
var linesP1 = parsePipeline(filepathP1);
var filepathP2 = $"Inputs/{solution.FilenameP2}";
var linesP2 = parsePipeline(filepathP2);

// run part one
Console.WriteLine("=== part one ===");
solution.Run(linesP1);

// reset and run part two
solution.Reset();
Console.WriteLine("=== part two ===");
solution.Run(linesP2, true);
