using AdventBase;

namespace AdventTestsBase;

public abstract class AdventSolutionTests<TSolution> where TSolution : Solution, new()
{
    protected TSolution GetSut() => new();
}