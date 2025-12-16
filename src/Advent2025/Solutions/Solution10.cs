using System.Numerics;
using AdventBase;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution10(ILogger<Solution10> logger) : Solution(2025, "10a", "2025-10.txt")
{
    public int MinimumPresses { get; private set; } = 0;
    
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        // an exercise in bit twiddling? yes please

        var machines = inputLines.Where(l => !string.IsNullOrEmpty(l))
            .Select(Parse).ToList();

        if (partTwo)
        {
            foreach (var m in machines)
            {
                MinimumPresses += GetMinimumPressesForJoltages(m);
            }
        }
        else
        {
            foreach (var m in machines)
            {
                MinimumPresses += GetMinimumPressesForLights(m);
            }
        }

        logger.LogInformation("Minimum presses for all machines: {MinimumPresses}", MinimumPresses);
    }

    public override void Reset()
    {
        MinimumPresses = 0;
    }

    public record Machine(uint GoalLights, uint[] Buttons, uint[] Joltages);

    public static Machine Parse(string input)
    {
        var split = input.Split(' ');

        // parse goal lights
        var lights = split[0][1..^1];
        var lightLength = lights.Length;
        var goalLights = 0u;
        for (var i = 0; i < lightLength; i++)
        {
            if (lights[i] == '#')
                goalLights ^= 1u << i;
        }

        // parse buttons
        var buttonStrings = split[1..^1];
        var buttonCount = buttonStrings.Length;
        var buttons = new uint[buttonCount];
        for (var i = 0; i < buttonCount; i++)
        {
            var buttonInts = buttonStrings[i][1..^1].Split(',').Select(int.Parse).ToList();
            foreach (var bi in buttonInts)
            {
                buttons[i] ^= 1u << bi;
            }
        }
        
        // parse joltages
        var joltageStrings = split[^1][1..^1].Split(',');
        var joltageCount = joltageStrings.Length;
        var joltages = new uint[joltageCount];
        for (var i = 0; i < joltageCount; i++)
        {
            // reverse order to make bitwise operations simpler
            joltages[i] = uint.Parse(joltageStrings[i]);
        }

        return new Machine(goalLights, buttons, joltages);
    }

    private int GetMinimumPressesForLights(Machine m)
    {
        // find minimum presses by brute force
        // not as efficient but sufficiently bit-twiddly
        var buttonCount = m.Buttons.Length;
        
        var minPresses = int.MaxValue;
        for (uint i = 0; i < Math.Pow(2, buttonCount); i++)
        {
            // for n buttons, will try 0, 1, 10, 11, 100, 101, 110...up to 2^n-1
            // for 8 buttons, that would be 11111111
            uint mask = i;
            uint indicator = 0;
            for (var b = 0; b < buttonCount && mask > 0; b++)
            {
                if ((mask & 1) == 1) indicator ^= m.Buttons[b];
                mask >>= 1;
            }

            if (indicator == m.GoalLights)
            {
                // that combo worked, how many presses did we make?
                var count = BitOperations.PopCount(i);
                if (count < minPresses) minPresses = count;
            }
        }

        return minPresses;
    }
    
    private int GetMinimumPressesForJoltages(Machine m)
    {
        // I don't know linear programming so we're just gonna wing it I guess
        logger.LogDebug("Joltages: {Joltages}", string.Join(",", m.Joltages));
        
        // first find the theoretical max presses for each button.
        // if a button is 1001 and our goal is {9,10,4,2}, max for that button is 2.

        var allButtons = new List<Button>();
        for (var i = 0; i < m.Buttons.Length; i++)
        {
            allButtons.Add(new Button(m.Buttons[i]));
        }

        // pick the most "consequential" buttons first to reduce search space.
        // we can find the most "impactful" by weighting their relative strength on every counter
        var counterTotals = new uint[m.Joltages.Length];
        foreach (var button in allButtons)
        {
            button.PushButton(counterTotals);
        }

        for (var i = 0; i < allButtons.Count; i++)
        {
            var weight = 0.0d;
            var freshCounters = new uint[m.Joltages.Length];
            allButtons[i].PushButton(freshCounters);
            for (var j = 0; j < m.Joltages.Length; j++)
            {
                if (freshCounters[j] == 0) continue;
                weight += (double)m.Joltages[j] / counterTotals[j];
            }

            allButtons[i].Weight = weight;
        }
        
        allButtons = allButtons.OrderByDescending(b => b.Weight).ToList();
        logger.LogDebug("Buttons weighted by impact: {Buttons}",
            string.Join(" | ", allButtons.Select(b => b.Mask.ToString($"B{m.Joltages.Length}"))));
        
        // now we can try to recurse for a solution
        var startingCounters = new uint[m.Joltages.Length];
        return RecurseForMinimum(m.Joltages, startingCounters, allButtons, 0, 0, int.MaxValue, new Dictionary<CounterKey, int>());
    }

    private int RecurseForMinimum(uint[] goal, uint[] current, List<Button> buttons, int buttonIdx, int currentPresses,
        int currentMinimumPresses, Dictionary<CounterKey, int> memo)
    {
        // short circuit if we're already over our minimum
        if (currentPresses >= currentMinimumPresses) return currentMinimumPresses;
        
        // short circuit if we've been in this state before for cheaper
        // var counterKey = HashCounterState(current, (ushort)buttonIdx);
        var counterKey = HashCounterState(current);
        if (memo.TryGetValue(counterKey, out var previousBest) &&
            previousBest + currentPresses >= currentMinimumPresses) return currentMinimumPresses;
        
        // next we can look ahead a bit to see if it's even possible to find a solution with remaining buttons
        // 1. find counters affected by remaining buttons
        // 2. find presses required yet for counters
        var counterDelta = new uint[goal.Length];
        for (var i = 0; i < goal.Length; i++)
        {
            if (goal[i] < current[i]) return currentMinimumPresses; // went over this counter
            counterDelta[i] = goal[i] - current[i];
        }

        var minimumStillRequired = counterDelta.Max();
        if (minimumStillRequired == 0)
        {
            // full solution found!
            return Math.Min(currentPresses, currentMinimumPresses);
        }
        var counterIdxsWithMax = counterDelta.Index().Where(t => t.Item == minimumStillRequired).Select(t => t.Index)
            .ToList();
        if (currentPresses + minimumStillRequired >= currentMinimumPresses)
            return currentMinimumPresses; // we know we can't beat it from here
        
        // which counters can still be affected here?
        var freshCounters = new uint[goal.Length];
        var myButtonAffectsAllMaxes = false;
        for (var i = buttonIdx; i < buttons.Count; i++)
        {
            buttons[i].PushButton(freshCounters);
            if (i == buttonIdx && counterIdxsWithMax.All(maxIdx => freshCounters[maxIdx] > 0))
                myButtonAffectsAllMaxes = true;
        }
        // if some goal is remaining but our buttons can't get to it, prune baby prune
        
        for (var i = 0; i < goal.Length; i++)
        {
            if (counterDelta[i] > 0 && freshCounters[i] == 0)
            {
                // if (_debug) Console.WriteLine($"b={buttonIdx} - Impossible to fill counter {i} requirement with remaining buttons");
                return currentMinimumPresses;
            }
        }
        
        // are there more buttons to push or was that the last button?
        if (buttonIdx >= buttons.Count) return currentMinimumPresses;
        
        // otherwise let's start pushing this next button
        // I think starting from the max of each button might be quicker to prune - maybe
        var currentMinimum = currentMinimumPresses;
        var myButton = buttons[buttonIdx];
        var maxPresses = myButton.MaxPressesForCounters(counterDelta);

        // for (var p = 0; p <= maxPresses; p++)
        for (var p = maxPresses; p >= 0; p--)
        {
            myButton.PushButton(current, p);

            var newLowerBound = currentPresses + p + (myButtonAffectsAllMaxes ? 0 : minimumStillRequired);
            if (newLowerBound >= currentMinimum) continue;
            
            var newMinimum = RecurseForMinimum(goal, current, buttons, buttonIdx + 1, currentPresses + p,
                currentMinimum, memo);
            
            if (newMinimum < currentMinimum)
            {
                currentMinimum = newMinimum;

                logger.LogDebug("b={Idx} -> found new minimum presses = {NewMin}", buttonIdx, newMinimum);
            }
            
            myButton.PushButton(current, p, true);
        }
        
        var bestRemainingFromHere = currentMinimum - currentPresses;
        if (bestRemainingFromHere < 0) bestRemainingFromHere = 0;
        // if (_debug) Console.WriteLine($"wrote {counterKey} => {bestRemainingFromHere} presses into memo");
        if (!memo.TryGetValue(counterKey, out var existing) || bestRemainingFromHere < existing)
            memo[counterKey] = bestRemainingFromHere;

        return currentMinimum;
    }

    public record Button
    {
        public uint Mask { get; init; }

        public uint[] CountersAffected { get; }

        public double Weight { get; set; } = 0d;

        public Button(uint mask)
        {
            Mask = mask;
            CountersAffected = new uint[10];
            PushButton(CountersAffected);
        }
        
        public int MaxPressesForCounters(uint[] goalCounters)
        {
            var min = CountersAffected.Take(goalCounters.Length)
                .Select((c, j) => c == 0 ? uint.MaxValue : goalCounters[j]).Min();
            var minBytes = BitConverter.GetBytes(min);
            return BitConverter.ToInt32(minBytes, 0);
        }
        
        public void PushButton(uint[] counters, int times = 1, bool unpush = false)
        {
            // handy util function that increments counters based on a bitmask
            // so applying "1001" to [5, 2, 1, 0] => [6, 2, 1, 1]
            for (var i = 0; i < times; i++)
            {
                var mask = Mask;
                while (mask > 0)
                {
                    var idx = BitOperations.TrailingZeroCount(mask); // applying 1001 -> first idx is 0;
                    if (unpush) counters[idx]--;
                    else counters[idx]++;
                    mask &= mask - 1; // applying 1001 -> mask becomes 1000, so 3 trailing zeroes -> next idx to ++ is 3.
                }
            }
        }
    }

    // private record struct CounterKey(ulong A, ulong B, ushort BtnIdx);
    private record struct CounterKey(ulong A, ulong B);

    // private CounterKey HashCounterState(uint[] arr, ushort btnIdx)
    // {
    //     ulong a;
    //     ulong b;
    //     Pack(arr, out a, out b);
    //     return new CounterKey(a, b, btnIdx);
    // }
    
    private CounterKey HashCounterState(uint[] arr)
    {
        ulong a;
        ulong b;
        Pack(arr, out a, out b);
        return new CounterKey(a, b);
    }
    
    // build state key hash for memoization
    private void Pack(uint[] counters, out ulong a, out ulong b)
    {
        ulong low = 0;
        ulong high = 0;
        var bit = 0;

        foreach (var c in counters)
        {
            if (bit < 64)
            {
                low |= (ulong)c << bit;
            }
            else
            {
                high |= (ulong)c << (bit - 64);
            }
            bit += 9;
        }

        a = low;
        b = high;
    }
}