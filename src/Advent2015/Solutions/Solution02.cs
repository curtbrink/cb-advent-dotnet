using AdventBase;

namespace Advent2015.Solutions;

public class Solution02() : Solution("2015-02.txt")
{
    public long Paper { get; private set; } = 0L;
    public long Ribbon { get; private set; } = 0L;

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        if (partTwo) return; // simultaneous

        foreach (var parts in inputLines.Where(l => !string.IsNullOrEmpty(l))
                     .Select(line => line.Split('x').Select(int.Parse).ToList()))
        {
            var (paper, ribbon) = GetWrappingRequirements(parts[0], parts[1], parts[2]);
            Paper += paper;
            Ribbon += ribbon;
        }

        Console.WriteLine($"I need {Paper} sqft of paper");
        Console.WriteLine($"I need {Ribbon} feet of ribbon");
    }

    public override void Reset()
    {
        // simultaneous part two
    }

    private static (int paper, int ribbon) GetWrappingRequirements(int length, int width, int height)
    {
        var lengthWidthArea = length * width;
        var lengthHeightArea = length * height;
        var widthHeightArea = width * height;
        var extraPaper = new List<int> { lengthWidthArea, lengthHeightArea, widthHeightArea }.Min();

        var lengthWidthPerim = 2 * length + 2 * width;
        var lengthHeightPerim = 2 * length + 2 * height;
        var widthHeightPerim = 2 * width + 2 * height;
        var ribbonWrap = new List<int> { lengthWidthPerim, lengthHeightPerim, widthHeightPerim }.Min();
        var ribbonBow = length * width * height;

        var paper = 2 * lengthWidthArea + 2 * lengthHeightArea + 2 * widthHeightArea + extraPaper;
        var ribbon = ribbonWrap + ribbonBow;

        return (paper, ribbon);
    }
}

/* These AoC 2015 solutions are converted from my original TypeScript implementations

export default function runSolution(fileInput: string): void {
  var presents = fileInput.trim().split("\n");
  var presentRequirements = presents.map((p) => {
    var dims = p.split("x").map((d) => parseInt(d));
    return getRequirements(dims[0], dims[1], dims[2]);
  });
  var totals = presentRequirements.reduce(
    (sums, currentPresent) => {
      sums.paper += currentPresent.paper;
      sums.ribbon += currentPresent.ribbon;
      return sums;
    },
    { paper: 0, ribbon: 0 }
  );
  console.log(`Total paper requirement is ${totals.paper}`);
  console.log(`Total ribbon requirement is ${totals.ribbon}`);
}

function getRequirements(
  x: number,
  y: number,
  z: number
): { paper: number; ribbon: number } {
  // paper reqs
  var lwa = x * y;
  var wha = y * z;
  var lha = z * x;
  var extra = Math.min(lwa, wha, lha);

  // ribbon reqs
  var lwp = 2 * x + 2 * y;
  var whp = 2 * y + 2 * z;
  var lhp = 2 * z + 2 * x;
  var ribbonWrap = Math.min(lwp, whp, lhp);
  var ribbonBow = x * y * z;

  return {
    paper: 2 * lwa + 2 * wha + 2 * lha + extra,
    ribbon: ribbonWrap + ribbonBow,
  };
}


*/