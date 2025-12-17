using AdventBase;
using AdventRunner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ======= SET THESE
var year = 2025;
var id = "12";
var debug = false;

// ======= DON'T CHANGE BELOW THIS LINE

var serviceCollection = new ServiceCollection();
serviceCollection.AddAdventYears(debug);
var sp = serviceCollection.BuildServiceProvider();
var logger = sp.GetRequiredService<ILogger<Program>>();

// look up solution
var solution = sp.GetRequiredService<AdventRegistry>().Years[year].Solutions[id];

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

logger.LogInformation("=== part one ===");
solution.Run(linesP1);

if (!solution.SinglePart)
{
    // reset and run part two
    var filepathP2 = $"Inputs/{solution.FilenameP2}";
    var linesP2 = parsePipeline(filepathP2);
    
    solution.Reset();
    logger.LogInformation("=== part two ===");
    solution.Run(linesP2, true);
}

Thread.Sleep(2000);