//-------------
// <copyright file="NUnitCrawler.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.NUnitRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Crawler integration with NUnit.
    /// </summary>
    /// <remarks>
    /// CrawlerService gives me links, starting from some base urls,
    /// matching a range of urls, containing a maximum number of links.
    /// It contains info about the links that have been visited
    /// and no info about the rest.
    /// I will then visit the rest, and get info about them.
    /// </remarks>
    [TestFixture]
    public abstract class NUnitCrawler
    {
        /// <summary>
        /// The default maximum number of links.
        /// </summary>
        private const int MaxLinksDefault = 10;

        /// <summary>
        /// The browser utility.
        /// </summary>
        private IBrowserUtility browser = new HtmlAgilityBrowserUtility();

        /// <summary>
        /// The crawler service.
        /// </summary>
        private ICrawlerService crawlerService;

        /// <summary>
        /// The text index.
        /// </summary>
        private int testIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitCrawler"/> class.
        /// </summary>
        protected NUnitCrawler()
        {
            this.crawlerService = new CrawlerService(
                new NullResultService(),
                this.browser);
        }

        /// <summary>
        /// Gets the acceptable status codes.
        /// </summary>
        /// <value>The acceptable status codes.</value>
        protected virtual IList<HttpStatusCode> AcceptableStatusCodes
        {
            get
            {
                var result = new List<HttpStatusCode>();
                result.Add(HttpStatusCode.OK);
                return result;
            }
        }

        /// <summary>
        /// Gets the max links.
        /// </summary>
        /// <value>The max links.</value>
        protected virtual int? MaxLinks
        {
            get
            {
                return MaxLinksDefault;
            }
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <returns>The hyper links.</returns>
        [Datapoints]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This might involve a database call or other long-running calls.")]
        public IEnumerable<HyperLink> GetLinks()
        {
            var startUrls = this.GetBaseUrls();
            var configuration =
                new CrawlConfiguration(
                startUrls,
                this.MaxLinks);
            var random = new Random();
            var result = this.crawlerService.GetLinks(configuration)
                .OrderBy(l => random.Next());
            if (this.MaxLinks.HasValue)
            {
                return result.Take(this.MaxLinks.Value).ToList();
            }

            return result;
        }

        /// <summary>
        /// Links the should be ok.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        [Theory]
        [Description("Tests that all links are OK")]
        public void LinkShouldBeOk(HyperLink link)
        {
            this.testIndex++;
            if (!link.IsVisited)
            {
                var result = this.browser.NavigateTo(link.Url);
                link.UpdateStatusCode(result.StatusCode);
            }

            string errorMessage = string.Format(
                CultureInfo.CurrentCulture,
                "Status code was {0} but was expected to be {1} for Url {2}",
                link.StatusCode,
                this.GetAcceptableStatusCodesString(),
                link.Url.AbsoluteUri);
            bool pageIsOk =
                this.AcceptableStatusCodes.Contains(link.StatusCode);
            Assert.That(
                pageIsOk,
                errorMessage);
            string successMessage = string.Format(
                CultureInfo.CurrentCulture,
                "OK: {0}/{1}: {2}",
                this.testIndex,
                this.MaxLinks,
                link.Url.AbsoluteUri);
            Console.WriteLine(successMessage);
        }

        /// <summary>
        /// Gets the base urls.
        /// </summary>
        /// <returns>A list of urls.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This might involve a database call or other long-running calls.")]
        protected abstract IList<Uri> GetBaseUrls();

        /// <summary>
        /// Gets the acceptable status codes string.
        /// </summary>
        /// <returns>A string representing the acceptable status codes.</returns>
        private string GetAcceptableStatusCodesString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var statusCode in this.AcceptableStatusCodes)
            {
                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    result.Append("/");
                }

                result.Append(statusCode);
            }

            return result.ToString();
        }
    }
}