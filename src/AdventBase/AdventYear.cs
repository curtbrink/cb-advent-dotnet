namespace AdventBase;

public abstract class AdventYear
{
    public abstract int Year { get; }
    
    public IDictionary<string, Solution> Solutions { get; }
    
    protected AdventYear(IEnumerable<Solution> solutions)
    {
        Solutions = solutions.Where(s => s.Year == Year).ToDictionary(s => s.Id);
    }

    public Solution GetSolution(string id) => Solutions[id];
}