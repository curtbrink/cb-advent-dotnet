using AdventBase;

namespace Advent2015.Solutions;

public class Solution01() : Solution("2015-01.txt", fileParseOption: SolutionParseOption.SingleLine)
{
    public int Floor { get; private set; } = 0;
    public int BasementFoundAtIndex { get; private set; } = -1;

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        if (partTwo) return; // simultaneous

        // really, there should only be one line
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line)) continue;

            for (var i = 0; i < line.Length; i++)
            {
                var p = line[i];
                var amount = p switch
                {
                    '(' => 1,
                    ')' => -1,
                    _ => throw new ArgumentOutOfRangeException(nameof(inputLines), "Invalid input"),
                };
                Floor += amount;
                if (Floor < 0 && BasementFoundAtIndex == -1)
                {
                    BasementFoundAtIndex = i;
                }
            }

            Console.WriteLine($"I ended up at floor {Floor}");
            Console.WriteLine($"I first hit the basement after {BasementFoundAtIndex + 1} moves");
        }
    }

    public override void Reset()
    {
        // all the magic happens in P1 anyway
    }
}

/* These AoC 2015 solutions are converted from my original TypeScript implementations

export default function runSolution(fileInput: string): void {
  // file input is one line of ('s and )'s
  // ( = increment
  // ) = decrement

  var floor = 0;
  var basementFound = false;
  let basementIdx;
  var idx = 0;
  for (var parenthesis of fileInput) {
    idx++;
    switch (parenthesis) {
      case "(":
        floor++;
        break;
      case ")":
        floor--;
        break;
    }
    if (!basementFound && floor < 0) {
      basementFound = true;
      basementIdx = idx;
    }
  }

  console.log(`The resulting floor is ${floor}`);
  console.log(`The first time the basement is entered is ${basementIdx}`);
}

*/