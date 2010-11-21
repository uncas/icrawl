namespace Uncas.WebTester.Infrastructure.Tests.Integration
{
    using NUnit.Framework;
    using Uncas.WebTester.Tests.Integration;

    [TestFixture]
    public class BaseSqlAdoTests
    {
        [Test]
        public void ExecuteScalar_DBNullResult_ReturnsNull()
        {
            object result = BaseSqlAdo.ExecuteScalar(
                IntegrationTestHelper.GetConnectionString(),
                "SELECT NULL");
            Assert.That(result, Is.Null);
        }
    }
}