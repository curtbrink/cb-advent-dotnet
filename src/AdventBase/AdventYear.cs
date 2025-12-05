namespace AdventBase;

public abstract class AdventYear;

public abstract class AdventYear<T> : AdventYear where T : AdventYear<T>
{
    public abstract Solution GetSolution(string id);
    public abstract bool IsOneLine(string id);
    public abstract string GetFileContents(string id);
}