namespace Uncas.WebTester.Infrastructure.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void GetCommandLineValue_NullArgs_Ok()
        {
            string value = CommandLineParser.GetCommandLineValue(null, "x");
            Assert.Null(value);
        }

        [Test]
        public void GetCommandLineValue_NoArgs_Ok()
        {
            string value = CommandLineParser.GetCommandLineValue(
                new string[] { }, "x");
            Assert.Null(value);
        }

        [Test]
        public void GetCommandLineValue_NameNotIncluded_Ok()
        {
            string value = CommandLineParser.GetCommandLineValue(
                new string[] { "-x" }, "y");
            Assert.Null(value);
        }

        [Test]
        public void GetCommandLineValue_NoValueAfterName_Ok()
        {
            string value = CommandLineParser.GetCommandLineValue(
                new string[] { "-x" }, "x");
            Assert.Null(value);
        }

        [Test]
        public void GetCommandLineValue_ValueAfterName_Ok()
        {
            string value = CommandLineParser.GetCommandLineValue(
                new string[] { "-x", "2" }, "x");
            Assert.That(value, Is.EqualTo("2"));
        }
    }
}