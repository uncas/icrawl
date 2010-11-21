namespace Uncas.WebTester.Tests.Unit.ApplicationServices
{
    using Moq;
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Repositories;

    [TestFixture]
    public class ResultServiceTests : ResultServiceTestsBase
    {
        protected override IResultService GetResultService()
        {
            IResultRepository resultRepository =
                new Mock<IResultRepository>().Object;
            return new ResultService(resultRepository);
        }
    }
}