using AdventBase;

namespace Advent2015.Solutions;

public class Solution06() : Solution("2015-06.txt")
{
    public int LightValue { get; private set; } = 0;

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        var grid = new int[1000, 1000];
        grid.Initialize();

        var instructions = inputLines.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => new Instruction(l, partTwo))
            .ToList();

        foreach (var instruction in instructions)
        {
            instruction.Execute(grid);
        }

        var totalLightValue = 0;
        foreach (var gridCell in grid)
        {
            totalLightValue += gridCell;
        }
        LightValue = totalLightValue;

        Console.WriteLine($"Total light value: {LightValue}");
    }

    public override void Reset()
    {
        LightValue = 0;
    }

    private record Instruction
    {
        public Type InstructionType { get; init; }
        public (int X, int Y) StartCoordinate { get; init; }
        public (int X, int Y) EndCoordinate { get; init; }

        private readonly bool _partTwo;

        public Instruction(string raw, bool partTwo = false)
        {
            _partTwo = partTwo;

            var parts = raw.Split(' ');
            InstructionType = parts.Length == 4 ? Type.Toggle : parts[1] == "on" ? Type.TurnOn : Type.TurnOff;

            StartCoordinate = ParseCoordinate(InstructionType == Type.Toggle ? parts[1] : parts[2]);
            EndCoordinate = ParseCoordinate(InstructionType == Type.Toggle ? parts[3] : parts[4]);
        }

        public void Execute(int[,] grid)
        {
            var a = GetAction();
            for (var y = StartCoordinate.Y; y <= EndCoordinate.Y; y++)
            {
                for (var x = StartCoordinate.X; x <= EndCoordinate.X; x++)
                {
                    a(grid, x, y);
                }
            }
        }

        private Action<int[,], int, int> GetAction() => InstructionType switch
        {
            Type.TurnOn when _partTwo => (grid, x, y) => Increment(grid, x, y, 1), 
            Type.TurnOn => TurnOn,
            Type.TurnOff when _partTwo => (grid, x, y) => Decrement(grid, x, y, 1), 
            Type.TurnOff => TurnOff,
            Type.Toggle when _partTwo => (grid, x, y) => Increment(grid, x, y, 2), 
            Type.Toggle => Toggle,
            _ => throw new ArgumentOutOfRangeException(nameof(InstructionType))
        };

        private static void TurnOn(int[,] grid, int x, int y)
        {
            grid[y, x] = 1;
        }
        
        private static void TurnOff(int[,] grid, int x, int y)
        {
            grid[y, x] = 0;
        }

        private static void Toggle(int[,] grid, int x, int y)
        {
            grid[y, x] = grid[y, x] == 1 ? 0 : 1;
        }
        
        private static void Increment(int[,] grid, int x, int y, int v)
        {
            grid[y, x] += v;
        }
        
        private static void Decrement(int[,] grid, int x, int y, int v)
        {
            grid[y, x] = Math.Max(0, grid[y, x] - v);
        }

        private static (int X, int Y) ParseCoordinate(string raw)
        {
            var coords = raw.Split(',').Select(int.Parse).ToList();
            return (coords[0], coords[1]);
        }

        public enum Type
        {
            TurnOn,
            TurnOff,
            Toggle,
        }
    }
}

/* These AoC 2015 solutions are converted from my original TypeScript implementations

(part one)

export default function runSolution(fileInput: string): void {
  var lightGrid = new LightGrid();

  var instructions = fileInput
    .trim()
    .split("\n")
    .map((l) => new Instruction(l));

  console.log(`Parsed ${instructions.length} light grid instructions`);

  var idx = 0;
  for (var ins of instructions) {
    idx++;
    console.log(`[${idx}] ... ${ins.toString()}`);
    console.log(`    prev: ${lightGrid.getNumberOfLightsOn()} on`);
    lightGrid.executeInstruction(ins);
    console.log(`     now: ${lightGrid.getNumberOfLightsOn()} on`);
  }
}

type InstructionType = "on" | "off" | "toggle";
type Coordinate = {
  x: number;
  y: number;
};
type InstructionMethod = (x: number, y: number) => void;
type InstructionMap = Record<InstructionType, InstructionMethod>;

function parseCoordinate(input: string): Coordinate {
  var splitPair = input.split(",").map((v) => parseInt(v));
  return { x: splitPair[0], y: splitPair[1] };
}

class Instruction {
  instructionType: InstructionType;
  coord1: Coordinate;
  coord2: Coordinate;

  constructor(input: string) {
    console.log(`Parsing instruction ${input}`);
    var tokens = input.split(" ");
    var idx = 0;

    // first token or two tokens determines type.
    if (tokens[idx] === "toggle") {
      this.instructionType = "toggle";
    } else {
      idx++;
      this.instructionType = tokens[idx] === "on" ? "on" : "off";
    }

    // next token determines first coordinate
    idx++;
    this.coord1 = parseCoordinate(tokens[idx]);

    // next token should be "through", error if not for debug purposes
    idx++;
    if (tokens[idx] !== "through") {
      throw new Error("something went wrong with parsing");
    }

    // last token determines second coordinate
    idx++;
    this.coord2 = parseCoordinate(tokens[idx]);

    console.log(this.toString());
  }

  toString(): string {
    return `[${this.instructionType}] | [${this.coord1.x}, ${this.coord1.y}] => [${this.coord2.x}, ${this.coord2.y}]`;
  }
}

class LightGrid {
  lights: boolean[][] = [];

  instructionMap: InstructionMap = {
    off: this.turnOff,
    on: this.turnOn,
    toggle: this.toggle,
  };

  constructor() {
    for (var i = 0; i < 1000; i++) {
      var lightRow = [];
      for (var j = 0; j < 1000; j++) {
        lightRow.push(false);
      }
      this.lights.push(lightRow);
    }

    console.log(
      `Initialized light grid with ${this.lights.length} rows of ${this.lights[0].length} lights each.`
    );
  }

  turnOn(x: number, y: number): void {
    this.lights[x][y] = true;
  }

  turnOff(x: number, y: number): void {
    this.lights[x][y] = false;
  }

  toggle(x: number, y: number): void {
    var current = this.lights[x][y];
    this.lights[x][y] = !current;
  }

  getNumberOfLightsOn(): number {
    var sum = 0;
    for (var row of this.lights) {
      for (var light of row) {
        sum += light ? 1 : 0;
      }
    }
    return sum;
  }

  executeInstruction(instruction: Instruction) {
    var instructionMethod =
      this.instructionMap[instruction.instructionType].bind(this);

    for (var i = instruction.coord1.x; i <= instruction.coord2.x; i++) {
      for (var j = instruction.coord1.y; j <= instruction.coord2.y; j++) {
        instructionMethod(i, j);
      }
    }
  }
}


(part two)

export default function runSolution(fileInput: string): void {
  var lightGrid = new LightGrid();

  var instructions = fileInput
    .trim()
    .split("\n")
    .map((l) => new Instruction(l));

  console.log(`Parsed ${instructions.length} light grid instructions`);

  var idx = 0;
  for (var ins of instructions) {
    idx++;
    console.log(`[${idx}] ... ${ins.toString()}`);
    console.log(`    prev: ${lightGrid.getTotalBrightness()} total brightness`);
    lightGrid.executeInstruction(ins);
    console.log(`     now: ${lightGrid.getTotalBrightness()} total brightness`);
  }
}

type InstructionType = "on" | "off" | "toggle";
type Coordinate = {
  x: number;
  y: number;
};
type InstructionMethod = (x: number, y: number) => void;
type InstructionMap = Record<InstructionType, InstructionMethod>;

function parseCoordinate(input: string): Coordinate {
  var splitPair = input.split(",").map((v) => parseInt(v));
  return { x: splitPair[0], y: splitPair[1] };
}

class Instruction {
  instructionType: InstructionType;
  coord1: Coordinate;
  coord2: Coordinate;

  constructor(input: string) {
    console.log(`Parsing instruction ${input}`);
    var tokens = input.split(" ");
    var idx = 0;

    // first token or two tokens determines type.
    if (tokens[idx] === "toggle") {
      this.instructionType = "toggle";
    } else {
      idx++;
      this.instructionType = tokens[idx] === "on" ? "on" : "off";
    }

    // next token determines first coordinate
    idx++;
    this.coord1 = parseCoordinate(tokens[idx]);

    // next token should be "through", error if not for debug purposes
    idx++;
    if (tokens[idx] !== "through") {
      throw new Error("something went wrong with parsing");
    }

    // last token determines second coordinate
    idx++;
    this.coord2 = parseCoordinate(tokens[idx]);

    console.log(this.toString());
  }

  toString(): string {
    return `[${this.instructionType}] | [${this.coord1.x}, ${this.coord1.y}] => [${this.coord2.x}, ${this.coord2.y}]`;
  }
}

class LightGrid {
  lights: number[][] = [];

  instructionMap: InstructionMap = {
    off: this.turnOff,
    on: this.turnOn,
    toggle: this.toggle,
  };

  constructor() {
    for (var i = 0; i < 1000; i++) {
      var lightRow = [];
      for (var j = 0; j < 1000; j++) {
        lightRow.push(0);
      }
      this.lights.push(lightRow);
    }

    console.log(
      `Initialized light grid with ${this.lights.length} rows of ${this.lights[0].length} lights each.`
    );
  }

  turnOn(x: number, y: number): void {
    this.lights[x][y] += 1;
  }

  turnOff(x: number, y: number): void {
    this.lights[x][y] -= 1;
    if (this.lights[x][y] < 0) {
      this.lights[x][y] = 0;
    }
  }

  toggle(x: number, y: number): void {
    this.lights[x][y] += 2;
  }

  getTotalBrightness(): number {
    var sum = 0;
    for (var row of this.lights) {
      for (var light of row) {
        sum += light;
      }
    }
    return sum;
  }

  executeInstruction(instruction: Instruction) {
    var instructionMethod =
      this.instructionMap[instruction.instructionType].bind(this);

    for (var i = instruction.coord1.x; i <= instruction.coord2.x; i++) {
      for (var j = instruction.coord1.y; j <= instruction.coord2.y; j++) {
        instructionMethod(i, j);
      }
    }
  }
}


*/