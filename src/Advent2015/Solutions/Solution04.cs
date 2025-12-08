using System.Security.Cryptography;
using System.Text;
using AdventBase;

namespace Advent2015.Solutions;

public class Solution04()
    : Solution("2015-04.txt", singlePart: true, fileParseOption: SolutionParseOption.SingleLineNoTrim)
{
    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        var key = inputLines.First();
        var hash = "";
        var idx = 0;
        while (!hash.StartsWith("00000"))
        {
            idx++;
            var val = $"{key}{idx}";
            hash = GetHash(val);
        }

        Console.WriteLine($"First number producing a 00000 hash: {idx} (hash = {hash})");

        while (!hash.StartsWith("000000"))
        {
            idx++;
            var val = $"{key}{idx}";
            hash = GetHash(val);
        }

        Console.WriteLine($"First number producing a 000000 hash: {idx} (hash = {hash})");
    }

    private static string GetHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = MD5.HashData(bytes);

        var s = new StringBuilder();
        foreach (var t in hash)
        {
            s.Append(t.ToString("x2"));
        }

        return s.ToString();
    }

    public override void Reset()
    {
    }
}

/* These AoC 2015 solutions are converted from my original TypeScript implementations

export default function runSolution(fileInput: string): void {
    var md5Key = fileInput;
    var seed = 0;

    var hash = "999999";
    while (hash.substring(0, 5) != "00000") {
        seed++;
        hash = Md5.hashStr(`${md5Key}${seed}`);
    }
    console.log("The lowest number that produces an MD5 with 5 leading zeroes is " + seed);

    seed = 0;
    hash = "999999";
    while (hash.substring(0, 6) != "000000") {
        seed++;
        hash = Md5.hashStr(`${md5Key}${seed}`);
    }
    console.log("The lowest number that produces an MD5 with 6 leading zeroes is " + seed);
}

*/