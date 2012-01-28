namespace Uncas.WebTester.ApplicationServices.Tests.Unit
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class CrawlConfigurationTests
    {
        [Test]
        public void CrawlConfiguration_WithStartUrls_ListIsPopulated()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            var url2 = new Uri("http://www2.uncas.dk");
            var urls = new Uri[] { url, url2 };

            // Act:
            var crawlConfiguration =
                new CrawlConfiguration(
                    urls, 10);

            // Assert:
            Assert.That(
                crawlConfiguration.MatchPatterns.Count(),
                Is.EqualTo(2));
        }

        [Test]
        public void CrawlConfiguration_WithPatterns_ListIsPopulated()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            var patterns = new string[] { "x", "y" };

            // Act:
            var crawlConfiguration =
                new CrawlConfiguration(
                    url,
                    10,
                    patterns);

            // Assert:
            Assert.That(
                crawlConfiguration.MatchPatterns.Count(),
                Is.EqualTo(3));
        }

        [Test]
        public void AddMatches_EmptyList_NoAdditional()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            var crawlConfiguration =
                new CrawlConfiguration(
                    url,
                    10);

            // Act:
            crawlConfiguration.AddMatches(null);

            // Assert:
            Assert.That(
                crawlConfiguration.MatchPatterns.Count(),
                Is.EqualTo(1));
        }

        [Test]
        public void AddMatches_TwoItemsInList_TwoAdditional()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            var crawlConfiguration =
                new CrawlConfiguration(
                    url,
                    10);

            // Act:
            crawlConfiguration.AddMatches(new string[] { "x", "y" });

            // Assert:
            Assert.That(
                crawlConfiguration.MatchPatterns.Count(),
                Is.EqualTo(3));
        }

        [Test]
        public void ToString_WithMatchPatterns_Ok()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            var crawlConfiguration =
                new CrawlConfiguration(
                    url,
                    10);

            // Act:
            string result = crawlConfiguration.ToString();

            // Assert:
            Assert.That(
                result,
                Is.StringContaining("http://www.uncas.dk"));
        }

        [Test]
        public void ChangeMaxVisits_To10_IsChangedTo10()
        {
            // Arrange:
            var url = new Uri("http://www.uncas.dk");
            const int NewMaxVisits = 117;
            var crawlConfiguration =
                new CrawlConfiguration(
                    url,
                    NewMaxVisits);

            // Assert:
            Assert.AreEqual(
                NewMaxVisits,
                crawlConfiguration.MaxVisits);
        }
    }
}
