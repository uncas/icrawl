namespace Uncas.WebTester.Infrastructure.Tests.Integration
{
    using NUnit.Framework;

    [TestFixture]
    public class ConfigFileParserTests
    {
        [Test]
        public void Parse_ExistingValue_Ok()
        {
            var value =
                ConfigFileParser.GetConfigValue(
                "website.port",
                "0");
            Assert.That(value, Is.Not.EqualTo("0"));
        }

        [Test]
        public void Parse_NonExistingValue_Ok()
        {
            var value =
                ConfigFileParser.GetConfigValue(
                "xxxwebsite.port",
                "0");
            Assert.That(value, Is.EqualTo("0"));
        }

        [Test]
        public void GetConnectionString_Existing_Ok()
        {
            var value = ConfigFileParser.GetConnectionStringFromConfigFile(
                "webTesterTestConnectionString", "x");
            Assert.That(value, Is.Not.EqualTo("x"));
        }

        [Test]
        public void GetConnectionString_NonExisting_Ok()
        {
            var value = ConfigFileParser.GetConnectionStringFromConfigFile(
                "webTesterTestConnqqweectionStradqweing", "x");
            Assert.That(value, Is.EqualTo("x"));
        }

        [Test]
        public void GetInt32Value_WhenConfigNameNotFound_ReturnsDefaultValue()
        {
            const int DefaultPort = 987;

            var value = ConfigFileParser.GetInt32Value("xxxwebsite.port", DefaultPort);

            Assert.That(value, Is.EqualTo(DefaultPort));
        }
    }
}