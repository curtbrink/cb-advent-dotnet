using AdventBase;

namespace Advent2015.Solutions;

public class Solution05() : Solution("2015-05.txt")
{
    public int NumberOfNice { get; private set; } = 0;

    public override void Run(List<string> inputLines, bool partTwo = false, bool debug = false)
    {
        NumberOfNice = partTwo switch
        {
            false => inputLines.Count(s =>
                s.HasThreeVowels() && s.HasRepeatedLetter() && !s.ContainsForbiddenStrings()),
            true => inputLines.Count(s => s.ContainsRepeatedDoubleLetter() && s.HasRepeatedLetterWithNBetween(1)),
        };
            
        Console.WriteLine($"There were {NumberOfNice} nice strings.");
    }

    public override void Reset()
    {
        NumberOfNice = 0;
    }
}

public static class Solution05StringExtensions
{
    // or, a small exercise in linq...
    extension(string s)
    {
        public bool HasThreeVowels() => s.ToCharArray().Count(c => c is 'a' or 'e' or 'i' or 'o' or 'u') >= 3;

        public bool HasRepeatedLetter() => s.HasRepeatedLetterWithNBetween(0);

        public bool HasRepeatedLetterWithNBetween(int n) => s.ToCharArray().Take(s.Length - (1 + n))
            .Select((c, idx) => (c: c, c1: s[idx + 1 + n]))
            .Any(tuple => tuple.c == tuple.c1);

        public bool ContainsForbiddenStrings() => new List<string> { "ab", "cd", "pq", "xy" }
            .Select(forbidden => s.IndexOf(forbidden, StringComparison.Ordinal)).Any(f => f != -1);

        // look at this absolute unit of recursive functional programming
        public bool ContainsRepeatedDoubleLetter() => s.Length >= 4 && (Enumerable.Range(2, s.Length - 3)
            .Select(n => (check: s[0..2], substr: s[n..(n + 2)]))
            .Any(tuple => tuple.check == tuple.substr) || s[1..].ContainsRepeatedDoubleLetter());
    }
}

// 85 lines of typescript vs 48 lines of linq-flavored C#... not bad

/* These AoC 2015 solutions are converted from my original TypeScript implementations

export default function runSolution(fileInput: string): void {
  var rulesPartOne: Rule[] = [hasThreeVowels, doubleLetter, passesBlacklist];
  var rulesPartTwo: Rule[] = [
    repeatedDoubleLetterNoOverlap,
    repeatedLetterSkipOne,
  ];

  var lines = fileInput.trim().split("\n");

  var partOneSum = 0;
  var partTwoSum = 0;
  for (var line of lines) {
    if (rulesPartOne.every((rule) => rule(line))) {
      partOneSum++;
    }
    if (rulesPartTwo.every((rule) => rule(line))) {
      partTwoSum++;
    }
  }

  console.log(
    `There are ${partOneSum} lines that pass all the rules for part one.`
  );
  console.log(
    `There are ${partTwoSum} lines that pass all the rules for part two.`
  );
}

type Rule = (input: string) => boolean;

function hasThreeVowels(input: string): boolean {
  var sum = 0;
  for (var letter of input) {
    if (["a", "e", "i", "o", "u"].includes(letter)) {
      sum++;
    }
    if (sum >= 3) {
      return true;
    }
  }
  return false;
}

function doubleLetter(input: string): boolean {
  for (var i = 0; i < input.length - 1; i++) {
    if (input[i] === input[i + 1]) {
      return true;
    }
  }
  return false;
}

function passesBlacklist(input: string): boolean {
  var bannedStrings = ["ab", "cd", "pq", "xy"];
  for (var bannedString of bannedStrings) {
    if (input.indexOf(bannedString) > -1) {
      return false;
    }
  }
  return true;
}

function repeatedDoubleLetterNoOverlap(input: string): boolean {
  if (input.length < 4) {
    return false;
  }

  for (var i = 0; i < input.length - 3; i++) {
    var checkString = input.substring(i, i + 2);
    for (var j = i + 2; j < input.length - 1; j++) {
      if (input.substring(j, j + 2) === checkString) {
        return true;
      }
    }
  }
  return false;
}

function repeatedLetterSkipOne(input: string): boolean {
  for (var i = 0; i < input.length - 2; i++) {
    if (input[i] === input[i + 2]) {
      return true;
    }
  }
  return false;
}


*/