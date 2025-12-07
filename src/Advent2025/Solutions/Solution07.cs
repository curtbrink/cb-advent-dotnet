using AdventBase;

namespace Advent2025.Solutions;

public class Solution07()
    : Solution("2025-07.txt", singlePart: true, fileParseOption: SolutionParseOption.MultiLineNoTrim)
{
    public int TotalSplits { get; private set; } = 0;
    public int TotalBeams => _currentBeams.Count;
    public long TotalTimelines => _beamIndexTimelines.Values.Sum();

    private HashSet<int> _currentBeams = [];
    private Dictionary<int, long> _beamIndexTimelines = new();

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        // find starting beam
        if (inputLines.Count == 0) throw new Exception("No input");

        // analyze too
        AnalyzeGrid(inputLines);

        var startBeam = inputLines[0].IndexOf('S');
        if (startBeam == -1) throw new Exception("No start beam");

        _beamIndexTimelines[startBeam] = 1;
        _currentBeams.Add(startBeam);

        // now for each line, iterate and pass beams down, or split if we find a ^
        for (var i = 1; i < inputLines.Count; i++)
        {
            if (string.IsNullOrEmpty(inputLines[i])) continue;
            var newBeams = new HashSet<int>();
            var newTimelines = new Dictionary<int, long>();
            // we only have to check where beams actually are
            foreach (var j in _currentBeams.ToList())
            {
                // init dict indexes to make sure we have at least 0's in there
                newTimelines.TryAdd(j - 1, 0);
                newTimelines.TryAdd(j, 0);
                newTimelines.TryAdd(j + 1, 0);
                
                switch (inputLines[i][j])
                {
                    case '.' when _currentBeams.Contains(j):
                        // pass beam on to next layer
                        newBeams.Add(j);
                        newTimelines[j] += _beamIndexTimelines[j];
                        break;
                    case '^' when _currentBeams.Contains(j):
                        // we assume we'll never go off the left or right side of the input grid
                        newBeams.Add(j - 1);
                        newBeams.Add(j + 1);
                        // if a beam had X paths to get here, each resulting unique beam from the splitter
                        // has an _additional_ X paths to get there...
                        newTimelines[j - 1] += _beamIndexTimelines[j];
                        newTimelines[j + 1] += _beamIndexTimelines[j];
                        TotalSplits++;
                        break;
                }
            }

            _currentBeams = newBeams;
            _beamIndexTimelines = newTimelines;
        }

        Console.WriteLine($"At the end, there's {TotalBeams} unique beams after {TotalSplits} total splits");
        Console.WriteLine($"This single particle exists on {TotalTimelines} timelines");
    }

    private void AnalyzeGrid(List<string> inputLines)
    {
        var oneString = string.Concat(inputLines);
        var totalSplitters = oneString.Count(c => c == '^');
        Console.WriteLine($" => I noticed there's a total of {totalSplitters} splitters");
    }

    public override void Reset()
    {
        TotalSplits = 0;
        _currentBeams.Clear();
    }
}