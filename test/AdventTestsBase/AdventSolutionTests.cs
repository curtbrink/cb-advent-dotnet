using AdventBase;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdventTestsBase;

public abstract class AdventSolutionTests<TSolution> where TSolution : Solution
{
    protected readonly ILogger<TSolution> Logger = new Mock<ILogger<TSolution>>().Object;
}