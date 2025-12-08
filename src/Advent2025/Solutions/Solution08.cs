using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AdventBase;

namespace Advent2025.Solutions;

public class Solution08() : Solution("2025-08.txt")
{
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        // initial thinking:
        // find distance between every pair: (1,000 * 999) / 2 (bidirectional link) = 499500 pairs. not too bad.
        // order by distance. pick the top 1000 pairs to connect. when connecting a pair, merge the circuit lists.
        // find largest 3 circuit lists. easy. part two is gonna be a big twist though isn't it?

        // ooh linq look at you
        List<JunctionBox> allNodes = (from line in inputLines
            where !string.IsNullOrEmpty(line)
            select line.Split(',').Select(long.Parse).ToList()
            into coords
            select new JunctionBox(coords[0], coords[1], coords[2])).ToList();

        var nodeDistances = new Dictionary<BidirTuple<JunctionBox>, double>();

        foreach (var node in allNodes)
        {
            foreach (var node2 in allNodes.Where(n => n != node))
            {
                // first check if pair is already calculated
                var tuple = new BidirTuple<JunctionBox>(node, node2);
                if (!nodeDistances.TryAdd(tuple, 0))
                {
                    // if (debug) Console.WriteLine($"Already found ({node} / {node2}) in distance dictionary");
                    continue;
                }

                var distance = node.Distance(node2);
                nodeDistances[tuple] = distance;
            }
        }

        var nShortestPairs = GetShortestPairs(nodeDistances, !partTwo ? 1000 : 0, debug);

        var numberOfBoxes = allNodes.Count;
        var circuits = new List<Circuit>();
        while (nShortestPairs.Count > 0)
        {
            var pair = nShortestPairs.Dequeue();

            // if (debug) Console.WriteLine($"Checking circuits for pair {pair}");

            var circuitWithA = circuits.Find(c => c.Nodes.Contains(pair.A));
            var circuitWithB = circuits.Find(c => c.Nodes.Contains(pair.B));
            var circuitHasA = circuitWithA != null;
            var circuitHasB = circuitWithB != null;

            if (!circuitHasA && !circuitHasB)
            {
                var newCircuit = new Circuit(pair);
                if (debug) Console.WriteLine($"Created new Circuit [{newCircuit.Id}] with {pair}");
                circuits.Add(new Circuit(pair));
            }
            else if (!circuitHasA && circuitHasB)
            {
                if (debug) Console.WriteLine($"Added {pair.A} to Circuit [{circuitWithB!.Id}]");
                circuitWithB!.Nodes.Add(pair.A);
            }
            else if (circuitHasA && !circuitHasB)
            {
                if (debug) Console.WriteLine($"Added {pair.B} to Circuit [{circuitWithA!.Id}]");
                circuitWithA!.Nodes.Add(pair.B);
            }
            else if (circuitHasA && circuitHasB)
            {
                // merge if not the same
                if (circuitWithA!.Id == circuitWithB!.Id)
                {
                    // if (debug)
                    //     Console.WriteLine(
                    //         $"Both {pair.A} and {pair.B} are already in same circuit [{circuitWithA!.Id}]");
                    continue;
                }

                circuitWithA.Nodes.AddRange(circuitWithB.Nodes);
                circuits = circuits.Where(c => c.Id != circuitWithB.Id).ToList();

                if (debug)
                {
                    Console.WriteLine(
                        $"Merged circuit [{circuitWithB.Id}] into circuit [{circuitWithA.Id}] and removed old circuit");
                    Console.WriteLine(
                        $"New circuit size of [{circuitWithA.Id}] is {circuitWithA.Nodes.Count} (looking for {numberOfBoxes})");
                }
            }

            // for part two here's where we'd do our check
            if (partTwo && ((circuitWithA != null && circuitWithA.Nodes.Count == numberOfBoxes) ||
                            (circuitWithB != null && circuitWithB.Nodes.Count == numberOfBoxes)))
            {
                // we have connected every box!
                Console.WriteLine("We have connected every box!");
                Console.WriteLine($"The linking pair that caused our graphs to connect was: {pair}");
                Console.WriteLine($"The X coords multiplied: {pair.A.X} * {pair.B.X} = {pair.A.X * pair.B.X}");
                return;
            }
        }

        if (partTwo)
        {
            Console.WriteLine("Wait ... we didn't find a complete graph??");
            return;
        }

        // now we want the top 3 circuits and we can do the same min heap approach
        var minHeapPq = new PriorityQueue<Circuit, int>();
        for (var i = 0; i < Math.Min(3, circuits.Count); i++)
        {
            minHeapPq.Enqueue(circuits[i], circuits[i].Nodes.Count);
        }

        var smallestCircuit = minHeapPq.Peek();
        for (var i = 3; i < circuits.Count; i++)
        {
            if (circuits[i].Nodes.Count <= smallestCircuit.Nodes.Count) continue;

            if (debug)
                Console.WriteLine(
                    $"Circuit {circuits[i].Id} has more nodes (n={circuits[i].Nodes.Count}) than {smallestCircuit.Id} (n={smallestCircuit.Nodes.Count})");
            minHeapPq.DequeueEnqueue(circuits[i], circuits[i].Nodes.Count);
            smallestCircuit = minHeapPq.Peek();
        }

        var largest = minHeapPq.UnorderedItems.ToList();
        Console.WriteLine(
            $"The 3 largest circuits have {largest[0].Priority}, {largest[1].Priority}, {largest[2].Priority} nodes.");
        var mult = largest[0].Priority * largest[1].Priority * largest[2].Priority;

        Console.WriteLine($"Multiplied, that's {mult}");
    }

    public override void Reset()
    {
    }

    private PriorityQueue<BidirTuple<JunctionBox>, double> GetShortestPairs(
        Dictionary<BidirTuple<JunctionBox>, double> distances, int n = 0, bool debug = false)
    {
        var nShortestPairs = new List<BidirTuple<JunctionBox>>();
        if (n > 0 && n < distances.Keys.Count)
        {
            // Priority queue is useful as a min-heap, but we want a max-heap because we want the _shortest_ distances.
            // but we can use a custom Comparer to reverse the order
            var maxHeapPq =
                new PriorityQueue<BidirTuple<JunctionBox>, double>(Comparer<double>.Create((x, y) => y.CompareTo(x)));

            // put the first n pairs on the queue
            // for part two, we don't want to limit, so we put all on the queue and then skip the heap part.
            var allPairs = distances.Keys.ToList();
            for (var i = 0; i < n; i++)
            {
                maxHeapPq.Enqueue(allPairs[i], distances[allPairs[i]]);
            }

            // test queue
            if (debug) PrintQueueDebug(maxHeapPq, distances);

            // now we have a max-heap of size n, so we can loop the rest of the node pairs
            // to find the n shortest distances of all possible pairs.
            var currentLongestPair = maxHeapPq.Peek();
            var currentLongestDistance = distances[currentLongestPair];
            for (var i = n; i < allPairs.Count; i++)
            {
                // if distance is shorter than the current longest, replace it
                var distance = distances[allPairs[i]];
                if (distance < currentLongestDistance)
                {
                    if (debug)
                    {
                        Console.WriteLine($"{distance} < {currentLongestDistance}");
                        Console.WriteLine($"Replaced {currentLongestPair} with {allPairs[i]}");
                    }

                    maxHeapPq.DequeueEnqueue(allPairs[i], distance);
                    currentLongestPair = maxHeapPq.Peek();
                    currentLongestDistance = distances[currentLongestPair];

                    if (debug) Console.WriteLine($"New longest distance is {currentLongestDistance}");
                }
            }

            // now our queue holds the n closest pairs. we want to turn our max heap queue into a min heap queue:
            nShortestPairs.AddRange(maxHeapPq.UnorderedItems.Select(i => i.Element));
        }
        else
        {
            // we want all of them, ordered by distance in a min heap priority queue
            nShortestPairs.AddRange(distances.Keys.ToList());
        }

        var minHeapPq = new PriorityQueue<BidirTuple<JunctionBox>, double>();
        foreach (var pair in nShortestPairs)
        {
            minHeapPq.Enqueue(pair, distances[pair]);
        }

        // and there we go - a queue whose Dequeue() gives us the next-shortest pair of nodes.
        return minHeapPq;
    }

    private void PrintQueueDebug(PriorityQueue<BidirTuple<JunctionBox>, double> queue,
        Dictionary<BidirTuple<JunctionBox>, double> distances)
    {
        // should print the first 1000 pairs smallest to largest distance.
        // make sure to add them back though :3 leave it as we found it...
        List<BidirTuple<JunctionBox>> addBack = [];
        while (queue.Count > 0)
        {
            var pair = queue.Dequeue();
            var dist = distances[pair];
            Console.WriteLine($"{dist} - {pair[0]} {pair[1]}");
            addBack.Add(pair);
        }

        Console.WriteLine($"Queue emptied (size={queue.Count}) - refilling!");

        foreach (var pair in addBack)
        {
            queue.Enqueue(pair, distances[pair]);
        }

        Console.WriteLine($"Queue back to size={queue.Count}");
    }

    private record JunctionBox(long X, long Y, long Z)
    {
        public double Distance(JunctionBox other) =>
            Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));

        public override string ToString() => $"[{X},{Y},{Z}]";
    }

    private class Circuit(BidirTuple<JunctionBox> pair)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public List<JunctionBox> Nodes { get; } = [pair.A, pair.B];
    }

    private struct BidirTuple<T>(T a, T b) : ITuple, IEquatable<BidirTuple<T>>
    {
        public T A => a;
        public T B => b;
        private ValueTuple<T, T> _tuple = ValueTuple.Create(a, b);
        private readonly ValueTuple<T, T> _inverseTuple = ValueTuple.Create(b, a);

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is BidirTuple<T> tuple && Equals(tuple);
        }

        // ensure bidirectionality here
        public override int GetHashCode() => 16269053 * _tuple.GetHashCode() + 16269053 * _inverseTuple.GetHashCode();

        public static bool operator ==(BidirTuple<T> left, BidirTuple<T> right) => left.Equals(right);

        public static bool operator !=(BidirTuple<T> left, BidirTuple<T> right) => !left.Equals(right);

        public bool Equals(BidirTuple<T> other)
        {
            return EqualityComparer<ValueTuple<T, T>>.Default.Equals(_tuple, other._tuple)
                   || EqualityComparer<ValueTuple<T, T>>.Default.Equals(_inverseTuple, other._tuple);
        }

        public override string ToString() => $"{{ {a} {b} }}";

        public object? this[int index] => index == 0 ? a : b;

        public int Length => 2;
    }
}