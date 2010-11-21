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
        /// The random number generator.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The result service.
        /// </summary>
        private readonly IResultService resultService;

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
        /// Gets the links.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        public IEnumerable<HyperLink> GetLinks(
            CrawlConfiguration configuration)
        {
            Guid batchNumber = Guid.NewGuid();
            List<HyperLink> links = GetStartUrls(configuration);
            int visits = 0;
            while (true)
            {
                var availableLinks = links.Where(l => !l.IsVisited);
                if (ShouldCrawlBreak(
                    configuration,
                    visits,
                    availableLinks.Count(),
                    links.Count))
                {
                    break;
                }

                visits++;
                this.HandleNextLink(
                    configuration,
                    links,
                    availableLinks,
                    batchNumber);
            }

            return links;
        }

        /// <summary>
        /// Gets the start urls.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        private static List<HyperLink> GetStartUrls(
            CrawlConfiguration configuration)
        {
            List<HyperLink> links = new List<HyperLink>();
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
            int? maxLinks = configuration.MaxLinks;
            return availableLinks == 0 ||
                (maxVisits.HasValue &&
                maxVisits.Value <= visits) ||
                (maxLinks.HasValue &&
                maxLinks.Value <= totalLinks);
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
            var navigateResult =
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
        /// Goes to the next nextLink.
        /// </summary>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <returns>The navigate result.</returns>
        private NavigateResult GoToNextLink(
            IEnumerable<HyperLink> availableLinks,
            Guid batchNumber)
        {
            int nextIndex = this.random.Next(availableLinks.Count());
            var nextLink = availableLinks.ElementAt(nextIndex);
            nextLink.UpdateBatchNumber(batchNumber);
            return NavigateHelper.NavigateToAndProcessLink(
                this.browserUtility,
                this.resultService,
                nextLink);
        }
    }
}