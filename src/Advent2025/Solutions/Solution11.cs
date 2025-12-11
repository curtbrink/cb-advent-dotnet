using AdventBase;

namespace Advent2025.Solutions;

public class Solution11() : Solution("2025-11.txt")
{
    public long NumberOfPaths { get; private set; } = 0L;
    public long NumberOfDacFftPaths { get; private set; } = 0L;
    
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        // parse once for devices
        // parse again to make the graph
        List<Device> allDevices = [];
        allDevices.Add(new Device("out"));
        
        var lines = inputLines.Where(line => !string.IsNullOrWhiteSpace(line)).Select(l => l.Split(':')).ToList();
        allDevices.AddRange(lines.Select(line => new Device(line[0])));

        foreach (var line in lines)
        {
            var id = line[0];
            var device = allDevices.Find(d => d.Id == id);
            if (device == null) throw new ArgumentOutOfRangeException(nameof(inputLines), "Invalid input");

            var outputs = line[1].Trim().Split(' ').ToList();
            var outDevices = outputs.Select(o => allDevices.Find(d => d.Id == o)).ToList();

            foreach (var outDevice in outDevices)
            {
                if (outDevice is null) throw new ArgumentOutOfRangeException(nameof(inputLines), "Invalid input");
                device.Outputs.Add(outDevice);
            }
        }

        if (!partTwo)
        {
            var you = allDevices.Find(d => d.Id == "you")!;
            PartOne(you, "out");
        }
        else
        {
            var svr = allDevices.Find(d => d.Id == "svr")!;
            var dac = allDevices.Find(d => d.Id == "dac")!;
            var fft = allDevices.Find(d => d.Id == "fft")!;
            PartTwo(svr, dac, fft, "out");
        }
    }

    private void PartOne(Device start, string end)
    {
        // take a stack. ~~leave a stack~~
        // take a stack, start at start, add all start's outputs to stack, pop next device dn, repeat...
        // stack search is basically just DFS without recursion
        var stack = new Stack<Device>();
        var numberOfOutPaths = 0L;
        stack.Push(start);
        
        // now just keep going until we run out of paths.
        while (stack.Count > 0)
        {
            var dev = stack.Pop();

            if (dev.Id == end)
            {
                numberOfOutPaths++;
                continue;
            }

            if (dev.Outputs.Count == 0) continue;

            foreach (var output in dev.Outputs)
            {
                stack.Push(output);
            }
        }

        NumberOfPaths = numberOfOutPaths;
        Console.WriteLine($"Total of {NumberOfPaths} paths from you -> out");
    }

    private void PartTwo(Device start, Device dac, Device fft, string end)
    {
        // stack approach doesn't work here anymore
        // solution: good ol recursion and memoization
        
        // plus through some careful research we know that all valid paths for this problem go svr->fft->dac->out
        // so we can break up the pathfinding into these checkpoints and multiply them together
        // i.e. if 8 paths from dac->out, and 4 paths from fft->dac, and 2 paths from svr->fft,
        // there are 2 * 4 * 8 = 64 total paths from svr->out that pass through fft and dac.

        var dacOutPaths = FindPaths(dac, end);
        Console.WriteLine($"There are {dacOutPaths} paths from dac->out");
        var fftDacPaths = FindPaths(fft, "dac");
        Console.WriteLine($"There are {fftDacPaths} paths from fft->dac");
        var startFftPaths = FindPaths(start, "fft");
        Console.WriteLine($"There are {startFftPaths} paths from svr->fft");

        NumberOfDacFftPaths = dacOutPaths * fftDacPaths * startFftPaths;

        Console.WriteLine($"For a total of {NumberOfDacFftPaths} paths!");
    }

    private long FindPaths(Device start, string endId)
    {
        var memo = new Dictionary<string, long>();
        return RecursePaths(start, endId, memo);
    }

    private long RecursePaths(Device next, string endId, Dictionary<string, long> memo)
    {
        // memo represents "paths from here to endId"
        if (next.Id == endId)
        {
            return 1;
        }

        if (memo.TryGetValue(next.Id, out var memoCount)) return memoCount;

        if (next.Outputs.Count == 0) return 0;

        var sum = 0L;
        foreach (var output in next.Outputs)
        {
            sum += RecursePaths(output, endId, memo);
        }

        memo[next.Id] = sum;
        return sum;
    }

    public override void Reset()
    {
    }

    public class Device(string id)
    {
        public string Id { get; } = id;

        public List<Device> Outputs { get; } = [];
    }
}