namespace Uncas.WebTester.Tests.Unit.ApplicationServices
{
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;

    [TestFixture]
    public class NullResultServiceTests : ResultServiceTestsBase
    {
        protected override IResultService GetResultService()
        {
            return new NullResultService();
        }
    }
}