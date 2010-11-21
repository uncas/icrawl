//-------------
// <copyright file="CrawlConfigurationParser.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Uncas.WebTester.ApplicationServices;

    /// <summary>
    /// Parses crawl configuration.
    /// </summary>
    public static class CrawlConfigurationParser
    {
        /// <summary>
        /// Parses the command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The crawl configuration.</returns>
        /// <remarks>
        /// Command line arguments, with default values:
        /// -url http://localhost -maxPages 10
        /// </remarks>
        public static CrawlConfiguration ParseArguments(
            IList<string> args)
        {
            string url = GetStartUrl(args);
            int? maxPages = GetMaxPages(args);

            var result = new CrawlConfiguration(
                new Uri(url),
                maxPages);
            string matches = CombinationParser.GetValue(
                args, "matches", "matches", string.Empty);
            if (!string.IsNullOrEmpty(matches))
            {
                string[] matchList =
                    matches.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                result.AddMatches(matchList);
            }

            return result;
        }

        /// <summary>
        /// Gets the maximum number of pages.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The maximum number of pages.</returns>
        private static int? GetMaxPages(IList<string> args)
        {
            const int MaxPagesDefault = 10;
            string maxPagesString =
                CombinationParser.GetValue(
                    args,
                    "maxPages",
                    "maxPages",
                    MaxPagesDefault.ToString(CultureInfo.InvariantCulture));
            int? maxPages = MaxPagesDefault;
            int maxPagesValue;
            if (!int.TryParse(maxPagesString, out maxPagesValue))
            {
                maxPagesValue = MaxPagesDefault;
            }

            maxPages = maxPagesValue;
            if (maxPagesValue <= 0)
            {
                maxPages = null;
            }

            return maxPages;
        }

        /// <summary>
        /// Gets the start URL.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The start URL.</returns>
        private static string GetStartUrl(IList<string> args)
        {
            const string UrlDefault = "http://localhost";
            string url = CombinationParser.GetValue(
                args,
                "url",
                "url",
                UrlDefault);
            return url;
        }
    }
}