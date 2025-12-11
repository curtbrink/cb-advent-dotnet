using AdventBase;
using Microsoft.Z3;

namespace Advent2025.Solutions;

public class Solution10Z3() : Solution("2025-10.txt")
{
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        if (!partTwo) return;

        var machines = inputLines.Where(l => !string.IsNullOrEmpty(l)).Select(Solution10.Parse).ToList();

        foreach (var machine in machines)
        {
            DoZ3Solve(machine);
        }
    }

    private void DoZ3Solve(Solution10.Machine m)
    {
        using var context = new Context();
        var optimize = context.MkOptimize();

        var ie = new IntExpr[m.Buttons.Length];

        for (int i = 0; i < m.Buttons.Length; i++)
        {
            ie[i] = context.MkIntConst($"press_{i}");
            optimize.Add(context.MkGe(ie[i], context.MkInt(0)));
        }
        
        var presses = new int[m.Buttons.Length, m.Joltages.Length];
        for (var b = 0; b < m.Buttons.Length; b++)
        {
            var btn = new Solution10.Button(m.Buttons[b]);
            var btnCounters = btn.CountersAffected;
            for (var i = 0; i < m.Joltages.Length; i++)
            {
                if (btnCounters[i] > 0)
                {
                    presses[b, i] = 1;
                }
            }
        }

        for (var i = 0; i < m.Joltages.Length; i++)
        {
            ArithExpr sum = context.MkInt(0);
            for (var j = 0; j < m.Buttons.Length; j++)
            {
                sum = context.MkAdd(sum, context.MkMul(context.MkInt(presses[j, i]), ie[j]));
            }

            optimize.Add(context.MkEq(sum, context.MkInt(m.Joltages[i])));
        }
        
        ArithExpr totalPresses = context.MkInt(0);
        for (var i = 0; i < m.Buttons.Length; i++)
            totalPresses = context.MkAdd(totalPresses, ie[i]);

        optimize.MkMinimize(totalPresses);

        if (optimize.Check() == Status.SATISFIABLE)
        {
            Console.WriteLine("Found solution");
        }
        else
        {
            Console.WriteLine("You fucked");
        }
    }

    public override void Reset()
    {
    }
}