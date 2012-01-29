//-------------
// <copyright file="HyperLink.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Models
{
    using System;
    using System.Globalization;
    using System.Net;

    /// <summary>
    /// Represents a hyper link.
    /// </summary>
    public class HyperLink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HyperLink"/> class.
        /// </summary>
        /// <param name="url">The URL of the link.</param>
        public HyperLink(Uri url)
        {
            this.Url = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperLink"/> class.
        /// </summary>
        /// <param name="url">The URL of the link.</param>
        /// <param name="referrer">The referrer.</param>
        public HyperLink(
            Uri url,
            Uri referrer)
            : this(url)
        {
            this.Referrer = referrer;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL of the link.</value>
        public Uri Url { get; private set; }

        /// <summary>
        /// Gets the referrer.
        /// </summary>
        /// <value>The referrer.</value>
        public Uri Referrer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is visited.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is visited; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisited { get; private set; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the load time.
        /// </summary>
        /// <value>The load time.</value>
        public TimeSpan LoadTime { get; private set; }

        /// <summary>
        /// Gets the number of links on this page.
        /// </summary>
        /// <value>The number of links.</value>
        public int NumberOfLinks { get; private set; }

        /// <summary>
        /// Gets the document elements.
        /// </summary>
        /// <value>The document elements.</value>
        public int DocumentElements { get; private set; }

        /// <summary>
        /// Gets the length of the HTML.
        /// </summary>
        /// <value>The length of the HTML.</value>
        public int HtmlLength { get; private set; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        public int Images { get; private set; }

        /// <summary>
        /// Gets the batch number.
        /// </summary>
        /// <value>The batch number.</value>
        public Guid BatchNumber { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}: {1}",
                this.IsVisited ? "Visited" : "Not visited",
                this.Url.AbsoluteUri);
        }

        /// <summary>
        /// Updates with the navigate result.
        /// </summary>
        /// <param name="loadTime">The load time.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="numberOfLinks">The number of links.</param>
        /// <param name="images">The images.</param>
        /// <param name="htmlLength">Length of the HTML.</param>
        /// <param name="documentElements">The document elements.</param>
        /// <param name="batchNumber">The batch number.</param>
        internal void UpdateWithNavigateResult(
            TimeSpan loadTime,
            HttpStatusCode statusCode,
            int numberOfLinks,
            int images,
            int htmlLength,
            int documentElements,
            Guid batchNumber)
        {
            this.BatchNumber = batchNumber;
            this.LoadTime = loadTime;
            this.StatusCode = statusCode;
            this.NumberOfLinks = numberOfLinks;
            this.Images = images;
            this.HtmlLength = htmlLength;
            this.DocumentElements = documentElements;
        }

        internal void Visit()
        {
            this.IsVisited = true;
        }
    }
}
