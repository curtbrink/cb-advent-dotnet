using System.Diagnostics;
using AdventBase;
using Csp.Core.Solvers.Shared.Interfaces;
using Csp.Core.Solvers.Shared.Models;
using Csp.Puzzles.Polyomino.Builders;
using Csp.Puzzles.Polyomino.Models;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution12(ILogger<Solution12> logger, ISearchSolver<Placement> backtracker) : Solution(2025, "12", "2025-12.txt")
{
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        List<Polyomino> pieces = [];
        // first 6 chunks of 5 lines are polyominoes we can parse
        for (var i = 0; i < 6; i++)
        {
            var startLine = 5 * i;
            var polyominoLines = inputLines[startLine..(startLine + 5)];

            var pId = polyominoLines[0].First().ToString();
            var poly = polyominoLines[1..4];
            // fifth line is empty

            pieces.Add(new Polyomino(pId, poly));
        }

        var testLines = inputLines.Skip(30).ToList();

        var successCount = 0;
        var totalCount = 0;
        var totalMs = 0L;
        foreach (var testLine in testLines)
        {
            var split = testLine.Split(": ");
            var gridSizes = split[0].Split('x').Select(int.Parse).ToArray();

            var polyBuilder = PolyominoBuilder.Create(gridSizes[0], gridSizes[1]);

            var quotas = split[1].Split(' ').Select(int.Parse).ToArray();
            for (var i = 0; i < quotas.Length; i++)
            {
                if (quotas[i] == 0) continue;

                polyBuilder.AddPolyomino(pieces[i], quotas[i]);
            }

            var builtPolyomino = polyBuilder.Build();

            var stopwatch = Stopwatch.StartNew();
            var polyResult = backtracker.Solve(builtPolyomino);
            var elapsed = stopwatch.ElapsedMilliseconds;

            logger.LogInformation("[{T}ms] Finished! {R}", elapsed, polyResult.Status.ToString());

            totalMs += elapsed;
            totalCount++;
            if (polyResult.Status == SolveStatus.Satisfied) successCount++;
        }

        logger.LogInformation("Finished all! Out of {Total} polyomino challenges, {Success} have a solution!",
            totalCount, successCount);
        logger.LogInformation("Total: {T} ms", totalMs);
    }

    public override void Reset()
    {
    }
}

// barebones backtrack, no specific polyomino heuristics or pruning:
// trial 1: 69.615 seconds
// trial 2: 76.857 seconds
// trial 3: 76.577 seconds

