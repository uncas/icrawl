//-------------
// <copyright file="CrawlerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Crawls a website.
    /// </summary>
    public class CrawlerService : ICrawlerService
    {
        /// <summary>
        /// The browser utility.
        /// </summary>
        private readonly IBrowserUtility browserUtility;

        /// <summary>
        /// The result service.
        /// </summary>
        private readonly IResultService resultService;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The lock object.
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// The visits.
        /// </summary>
        private int visits = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlerService"/> class.
        /// </summary>
        /// <param name="resultService">The result service.</param>
        /// <param name="browserUtility">The browser utility.</param>
        public CrawlerService(
            IResultService resultService,
            IBrowserUtility browserUtility)
        {
            this.browserUtility = browserUtility;
            this.resultService = resultService;
        }

        /// <summary>
        /// Crawls the website.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Crawl(CrawlConfiguration configuration)
        {
            this.GetLinks(configuration);
        }

        /// <summary>
        /// Gets the start urls.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        private static IList<HyperLink> GetStartUrls(
            CrawlConfiguration configuration)
        {
            IList<HyperLink> links = new List<HyperLink>();
            foreach (Uri startUrl in configuration.StartUrls)
            {
                links.Add(new HyperLink(startUrl));
            }

            return links;
        }

        /// <summary>
        /// Determines whether the crawl should break.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="visits">The number of pages.</param>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="totalLinks">The total links.</param>
        /// <returns>
        /// True if the crawl should break, false otherwise.
        /// </returns>
        private static bool ShouldCrawlBreak(
            CrawlConfiguration configuration,
            int visits,
            int availableLinks,
            int totalLinks)
        {
            int? maxVisits = configuration.MaxVisits;
            return availableLinks == 0 ||
                (maxVisits.HasValue &&
                maxVisits.Value <= visits);
        }

        /// <summary>
        /// Adds the new links.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="currentLinks">The links for the current page.</param>
        /// <param name="configuration">The configuration.</param>
        private static void AddNewLinks(
            IList<HyperLink> links,
            IEnumerable<HyperLink> currentLinks,
            CrawlConfiguration configuration)
        {
            var newLinks = currentLinks.Where(
                l => !links.Any(l2 => l2.Url == l.Url) &&
                l.Url != null &&
                configuration.MatchesPattern(l.Url));
            foreach (var newLink in newLinks)
            {
                links.Add(newLink);
            }
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        private IEnumerable<HyperLink> GetLinks(
            CrawlConfiguration configuration)
        {
            Guid batchNumber = Guid.NewGuid();
            IList<HyperLink> links = GetStartUrls(configuration);

            // TODO: Parallellize using this tip:
            // http://stackoverflow.com/questions/8671771/whats-the-best-way-of-achieving-a-parallel-infinite-loop
            while (true)
            {
                bool continueLoop = this.ContinueLoop(links, configuration, batchNumber);
                if (!continueLoop)
                {
                    break;
                }
            }

            return links;
        }

        /// <summary>
        /// Continues the loop.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <returns>True to continue loop, otherwise false.</returns>
        private bool ContinueLoop(
            IList<HyperLink> links,
            CrawlConfiguration configuration,
            Guid batchNumber)
        {
            IEnumerable<HyperLink> availableLinks = links.Where(l => !l.IsVisited);
            if (ShouldCrawlBreak(
                configuration,
                this.visits,
                availableLinks.Count(),
                links.Count))
            {
                return false;
            }

            this.visits++;
            this.HandleNextLink(
                configuration,
                links,
                availableLinks,
                batchNumber);

            return true;
        }

        /// <summary>
        /// Handles the next link.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="links">The links.</param>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="batchNumber">The batch number.</param>
        private void HandleNextLink(
            CrawlConfiguration configuration,
            IList<HyperLink> links,
            IEnumerable<HyperLink> availableLinks,
            Guid batchNumber)
        {
            NavigateResult navigateResult =
                this.GoToNextLink(
                availableLinks,
                batchNumber);
            if (navigateResult != null)
            {
                AddNewLinks(
                    links,
                    navigateResult.Links,
                    configuration);
            }
        }

        /// <summary>
        /// Goes to the next link.
        /// </summary>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <returns>The navigate result.</returns>
        private NavigateResult GoToNextLink(
            IEnumerable<HyperLink> availableLinks,
            Guid batchNumber)
        {
            int nextIndex = this.random.Next(availableLinks.Count());
            HyperLink nextLink = availableLinks.ElementAt(nextIndex);
            NavigateResult result =
                NavigateHelper.NavigateToAndProcessUrl(
                nextLink.Url,
                this.browserUtility);
            nextLink.UpdateWithNavigateResult(
                result.LoadTime,
                result.StatusCode,
                result.NumberOfLinks,
                result.Images,
                result.HtmlLength,
                result.DocumentElements,
                batchNumber);
            this.resultService.ProcessResult(nextLink);
            return result;
        }
    }
}