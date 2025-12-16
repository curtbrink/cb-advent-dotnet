using AdventBase;
using Microsoft.Extensions.Logging;
using Microsoft.Z3;

namespace Advent2025.Solutions;

public class Solution10Z3(ILogger<Solution10Z3> logger) : Solution(2025, "10b", "2025-10.txt")
{
    // note: for this to run/build you need Microsoft.Z3.dll in <solution root>/lib directory.
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        if (!partTwo) return;

        var machines = inputLines.Where(l => !string.IsNullOrEmpty(l)).Select(Solution10.Parse).ToList();

        var pressTotal = 0;
        foreach (var machine in machines)
        {
            pressTotal += DoZ3Solve(machine);
        }

        logger.LogInformation("Total presses for all machines: {PressTotal}", pressTotal);
    }

    private int DoZ3Solve(Solution10.Machine m)
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
            logger.LogDebug("Found solution");
            
            var model = optimize.Model;

            var total = 0;

            for (var i = 0; i < m.Buttons.Length; i++)
            {
                var v = ((IntNum)model.Evaluate(ie[i])).Int;
                total += v;
                logger.LogDebug("Button {I}: {V}", i, v);
            }

            logger.LogDebug("Total presses: {Total}", total);

            return total;
        }

        logger.LogDebug("No solution found :(");

        throw new Exception("No solution found");
    }

    public override void Reset()
    {
    }
}