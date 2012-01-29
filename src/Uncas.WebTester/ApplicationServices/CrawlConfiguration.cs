//-------------
// <copyright file="CrawlConfiguration.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Configuration for crawling.
    /// </summary>
    public class CrawlConfiguration
    {
        private const int DefaultMaxVisits = 1000;

        /// <summary>
        /// The match patterns.
        /// </summary>
        private List<string> matchPatterns;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlConfiguration"/> class.
        /// </summary>
        /// <param name="startUrls">The start urls.</param>
        /// <param name="maxVisits">The max visits.</param>
        public CrawlConfiguration(
            IList<Uri> startUrls,
            int? maxVisits)
        {
            this.StartUrls = startUrls;
            this.MaxVisits = maxVisits ?? DefaultMaxVisits;
            this.SetMatchPatterns();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlConfiguration"/> class.
        /// </summary>
        /// <param name="startUrl">The start URL.</param>
        /// <param name="maxVisits">The max visits.</param>
        /// <param name="patterns">The patterns.</param>
        public CrawlConfiguration(
            Uri startUrl,
            int? maxVisits,
            params string[] patterns)
        {
            this.StartUrls = new List<Uri>();
            this.StartUrls.Add(startUrl);
            this.SetMatchPatterns();
            this.MaxVisits = maxVisits ?? DefaultMaxVisits;
            if (patterns != null && patterns.Length > 0)
            {
                this.matchPatterns.AddRange(patterns);
            }
        }

        /// <summary>
        /// Gets the start urls.
        /// </summary>
        /// <value>The start urls.</value>
        public IList<Uri> StartUrls { get; private set; }

        /// <summary>
        /// Gets the match patterns.
        /// </summary>
        /// <value>The match patterns.</value>
        public IEnumerable<string> MatchPatterns
        {
            get
            {
                return this.matchPatterns;
            }
        }

        /// <summary>
        /// Gets the max number of pages to visit.
        /// </summary>
        /// <value>The max number of pages to visit.</value>
        public int MaxVisits { get; private set; }

        /// <summary>
        /// Adds the matches.
        /// </summary>
        /// <param name="matchList">The match list.</param>
        public void AddMatches(IEnumerable<string> matchList)
        {
            if (matchList != null)
            {
                this.matchPatterns.AddRange(matchList);
            }
        }

        /// <summary>
        /// Matches the pattern.
        /// </summary>
        /// <param name="url">The given URL.</param>
        /// <returns>True if any pattern matches, otherwise false.</returns>
        public bool MatchesPattern(Uri url)
        {
            return this.MatchPatterns.Any(
                mp => url.AbsoluteUri.StartsWith(
                    mp,
                    StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(
                CultureInfo.CurrentCulture,
                "Start url: {0}, max {1} pages.",
                this.StartUrls.FirstOrDefault(),
                this.MaxVisits);
            if (this.matchPatterns.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("Matches:");
                foreach (string matchPattern in this.matchPatterns)
                {
                    builder.AppendFormat(
                        CultureInfo.CurrentCulture,
                        "- {0}",
                        matchPattern);
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Sets the match patterns.
        /// </summary>
        private void SetMatchPatterns()
        {
            this.matchPatterns = new List<string>();
            foreach (Uri url in this.StartUrls)
            {
                this.matchPatterns.Add(url.AbsoluteUri);
            }
        }
    }
}