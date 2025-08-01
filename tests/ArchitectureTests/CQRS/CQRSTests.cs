using Application.Abstractions.Messaging;
using NetArchTest.Rules;
using Shouldly;
using TestResult = NetArchTest.Rules.TestResult;

namespace ArchitectureTests.CQRS;

public class CQRSTests : BaseTest
{
    [Fact]
    public void QueryHandlers_ShouldNotDependOn_IApplicationDbContext()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .ShouldNot()
            .HaveDependencyOn("IApplicationDbContext")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
