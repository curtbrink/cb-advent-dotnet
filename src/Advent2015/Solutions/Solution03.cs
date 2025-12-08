using AdventBase;

namespace Advent2015.Solutions;

public class Solution03() : Solution("2015-03.txt", fileParseOption: SolutionParseOption.SingleLineNoTrim)
{
    public long HousesVisited => _housesVisited.Count;

    private readonly HashSet<ValueTuple<int, int>> _housesVisited = [];

    private (int x, int y) _santaCoords = (0, 0);
    private (int x, int y) _robosantaCoords = (0, 0);

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        _housesVisited.Add(ValueTuple.Create(0, 0));

        var instructions = inputLines.First();

        var idx = 0;
        foreach (var c in instructions)
        {
            (int x, int y) vector = c switch
            {
                '^' => (0, -1),
                '>' => (1, 0),
                'v' => (0, 1),
                '<' => (-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(c), "Invalid instruction"),
            };

            if (partTwo && idx % 2 == 1)
            {
                _robosantaCoords = (_robosantaCoords.x + vector.x, _robosantaCoords.y + vector.y);
                _housesVisited.Add(ValueTuple.Create(_robosantaCoords.x, _robosantaCoords.y));
            }
            else
            {
                _santaCoords = (_santaCoords.x + vector.x, _santaCoords.y + vector.y);
                _housesVisited.Add(ValueTuple.Create(_santaCoords.x, _santaCoords.y));
            }

            idx++;
        }

        Console.WriteLine($"A total of {HousesVisited} houses were visited");
    }

    public override void Reset()
    {
        _housesVisited.Clear();
        _santaCoords = (0, 0);
        _robosantaCoords = (0, 0);
    }
}

/* These AoC 2015 solutions are converted from my original TypeScript implementations

export default function runSolution(fileInput: string): void {
  var makeXYKey = (x: number, y: number) => `${x}|${y}`;
  var visitedHousesPart1 = new Set();
  var visitedHousesPart2 = new Set();

  // starting house
  var x = 0;
  var y = 0;
  // part 2 vars
  var santaX = 0;
  var santaY = 0;
  var roboX = 0;
  var roboY = 0;
  visitedHousesPart1.add(makeXYKey(x, y));
  visitedHousesPart2.add(makeXYKey(x, y));

  for (var i = 0; i < fileInput.length; i++) {
    var transform = { x: 0, y: 0 };
    switch (fileInput[i]) {
      case ">":
        transform.x = 1;
        break;
      case "v":
        transform.y = 1;
        break;
      case "<":
        transform.x = -1;
        break;
      case "^":
        transform.y = -1;
        break;
    }

    // update part 1 vars every time
    x += transform.x;
    y += transform.y;
    visitedHousesPart1.add(makeXYKey(x, y));

    // update part 2 vars based on idx
    if (i % 2 === 0) {
      santaX += transform.x;
      santaY += transform.y;
      visitedHousesPart2.add(makeXYKey(santaX, santaY));
    } else {
      roboX += transform.x;
      roboY += transform.y;
      visitedHousesPart2.add(makeXYKey(roboX, roboY));
    }
  }

  console.log(`Santa visited ${visitedHousesPart1.size} houses`);
  console.log(
    `Alternatively, if Santa was using Robo-Santa, they collectively visited ${visitedHousesPart2.size} houses`
  );
}


*/