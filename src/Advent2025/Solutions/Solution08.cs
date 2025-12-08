using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AdventBase;

namespace Advent2025.Solutions;

public class Solution08() : Solution("2025-08.txt")
{
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        // find distance between every pair: (1,000 * 999) / 2 (bidirectional link) = 499500 pairs. not too bad.
        // order by distance via a max heap priority queue. pick the top n pairs to connect.
        // -> part one: n = 1000
        // -> part two: keep going until entire graph is connected
        // when connecting a pair, merge the circuit lists.
        // -> part one, find largest 3 circuit lists.
        // -> part two, find the pair that causes the last circuit to contain all nodes.

        // ooh linq look at you go!
        List<JunctionBox> allNodes = (from line in inputLines
            where !string.IsNullOrEmpty(line)
            select line.Split(',').Select(long.Parse).ToList()
            into coords
            select new JunctionBox(coords[0], coords[1], coords[2])).ToList();

        var distanceQueue = new PriorityQueue<BidirTuple<JunctionBox>, double>();
        var pairsAdded = new HashSet<BidirTuple<JunctionBox>>(); // only want (a,b) and not (b,a).

        foreach (var node in allNodes)
        {
            foreach (var node2 in allNodes.Where(n => n != node))
            {
                // first check if pair is already calculated
                var tuple = new BidirTuple<JunctionBox>(node, node2);
                if (!pairsAdded.Add(tuple)) continue;
                
                // if not, find its distance and enqueue
                var distance = node.Distance(node2);
                distanceQueue.Enqueue(tuple, distance);
            }
        }
        
        var numberOfBoxes = allNodes.Count;
        var circuits = new List<Circuit>();
        var pairsChecked = 0;
        while (distanceQueue.Count > 0 && (partTwo || pairsChecked < 1000))
        {
            pairsChecked++;
            var pair = distanceQueue.Dequeue();

            var circuitWithA = circuits.Find(c => c.Nodes.Contains(pair.A));
            var circuitWithB = circuits.Find(c => c.Nodes.Contains(pair.B));

            if (circuitWithA is null && circuitWithB is null)
            {
                var newCircuit = new Circuit(pair);
                if (debug) Console.WriteLine($"Created new Circuit [{newCircuit.Id}] with {pair}");
                circuits.Add(new Circuit(pair));
            }
            else if (circuitWithA is null)
            {
                if (debug) Console.WriteLine($"Added {pair.A} to Circuit [{circuitWithB!.Id}]");
                circuitWithB!.Nodes.Add(pair.A);
            }
            else if (circuitWithB is null)
            {
                if (debug) Console.WriteLine($"Added {pair.B} to Circuit [{circuitWithA.Id}]");
                circuitWithA.Nodes.Add(pair.B);
            }
            else
            {
                // merge if not the same
                if (circuitWithA.Id == circuitWithB.Id)
                {
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

            // for part two, if we have a complete graph, we're good
            if (partTwo && ((circuitWithA != null && circuitWithA.Nodes.Count == numberOfBoxes) ||
                            (circuitWithB != null && circuitWithB.Nodes.Count == numberOfBoxes)))
            {
                Console.WriteLine("We have connected every box!");
                Console.WriteLine($"The linking pair that caused our circuit to contain every box was: {pair}");
                Console.WriteLine($"The X coordinates multiplied: {pair.A.X} * {pair.B.X} = {pair.A.X * pair.B.X}");
                return;
            }
        }

        if (partTwo)
        {
            Console.WriteLine("Wait ... somehow we connected every possible pair and didn't make a complete graph. :(");
            return;
        }

        // now we want the top 3 circuits, and we can use a max heap this time
        var maxHeapPq = new PriorityQueue<Circuit, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        foreach (var c in circuits)
        {
            maxHeapPq.Enqueue(c, c.Nodes.Count);
        }

        var largest = new List<int>();
        for (var i = 0; i < 3; i++)
        {
            largest.Add(maxHeapPq.Dequeue().Nodes.Count);
        }
        Console.WriteLine(
            $"The 3 largest circuits have {largest[0]}, {largest[1]}, {largest[2]} nodes.");
        var mult = largest[0] * largest[1] * largest[2];

        Console.WriteLine($"Multiplied, that's {mult}");
    }

    public override void Reset()
    {
        // not stateful, this one
    }

    private record JunctionBox(long X, long Y, long Z)
    {
        public double Distance(JunctionBox other) =>
            Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));

        public override string ToString() => $"[{X},{Y},{Z}]";
    }

    private class Circuit(BidirTuple<JunctionBox> pair)
    {
        public Guid Id { get; } = Guid.NewGuid();
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
            var ec = EqualityComparer<ValueTuple<T, T>>.Default;
            return ec.Equals(_tuple, other._tuple) || ec.Equals(_inverseTuple, other._tuple);
        }

        public override string ToString() => $"{{ {a} {b} }}";

        public object? this[int index] => index == 0 ? a : b;

        public int Length => 2;
    }
}