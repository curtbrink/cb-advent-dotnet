using AdventBase;

namespace AdventRunner;

public class AdventRegistry
{
    public IDictionary<int, AdventYear> Years { get; }

    public AdventRegistry(IEnumerable<AdventYear> years)
    {
        Years = years.ToDictionary(k => k.Year);
    }
}