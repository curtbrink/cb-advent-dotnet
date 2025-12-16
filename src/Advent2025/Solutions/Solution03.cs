using AdventBase;
using Microsoft.Extensions.Logging;

namespace Advent2025.Solutions;

public class Solution03(ILogger<Solution03> logger) : Solution(2025, "03", "2025-03.txt")
{
    public long MaxJoltage { get; private set; } = 0;
    
    public override void Run(List<string> inputLines, bool partTwo = false)
    {
        var batteryQuota = partTwo ? 12 : 2;
        
        foreach (var bank in inputLines)
        {
            MaxJoltage += GetMaxForBankWithNBatteries(bank, batteryQuota);
        }

        logger.LogInformation("Max joltage available from all {Inputs} battery banks is: {MaxJoltage}",
            inputLines.Count, MaxJoltage);
    }

    private long GetMaxForBankWithNBatteries(string bank, int n)
    {
        // keep a list of int[n]
        var chosenBatteries = new int[n];
        for (var nInit = 0; nInit < n; nInit++)
        {
            chosenBatteries[nInit] = -1;
        }
        
        var banklen = bank.Length;
        
        // find tens
        for (var i = 0; i < banklen; i++)
        {
            var num = int.Parse([bank[i]]);
            
            // check each position. skip if too far to the end, or already at 9
            // if we update a position, everything to the right has to be zeroed out
            var positionUpdated = false;
            for (var j = 0; j < n; j++)
            {
                if (positionUpdated)
                {
                    chosenBatteries[j] = -1;
                    continue;
                }
                // e.g. looking at banklen=5, 3 batteries, battery 0 must be idx <=2
                var maxIdx = banklen - n + j;
                if (chosenBatteries[j] != 9 && i <= maxIdx && num > chosenBatteries[j])
                {
                    chosenBatteries[j] = num;
                    positionUpdated = true;
                }
            }
        }

        return long.Parse(string.Join("", chosenBatteries));
    }

    public override void Reset()
    {
        MaxJoltage = 0;
    }
}