namespace AdventBase;

public abstract class AdventYear
{
    public abstract Solution GetSolution(string id);
}

public abstract class AdventYear<T> : AdventYear where T : AdventYear<T>;