//-------------
// <copyright file="HtmlAgilityBrowserUtility.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using HtmlAgilityPack;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Navigates using web request and parses using HtmlAgilityPack.
    /// </summary>
    public class HtmlAgilityBrowserUtility : IBrowserUtility
    {
        /// <summary>
        /// Navigates to the given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <returns>The navigate result.</returns>
        public NavigateResult NavigateTo(Uri url)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WebRequest request = GetRequest(url);
            HttpWebResponse response = GetResponse(request);
            NavigateResult result = GetResult(url, response);
            stopwatch.Stop();
            result.AddLoadTime(stopwatch.Elapsed);
            return result;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        private static HttpWebResponse GetResponse(WebRequest request)
        {
            if (request == null)
            {
                return null;
            }

            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                return (HttpWebResponse)ex.Response;
            }
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <param name="url">The URL of the page.</param>
        /// <returns>The request.</returns>
        private static WebRequest GetRequest(Uri url)
        {
            try
            {
                return WebRequest.Create(url);
            }
            catch (NotSupportedException)
            {
            }

            return null;
        }

        /// <summary>
        /// Populates the result.
        /// </summary>
        /// <param name="url">The URL to populate from.</param>
        /// <param name="response">The response.</param>
        /// <returns>The navigate result.</returns>
        private static NavigateResult GetResult(
            Uri url,
            HttpWebResponse response)
        {
            if (response == null)
            {
                return NavigateResult.Empty;
            }

            HttpStatusCode statusCode = response.StatusCode;
            var responseStream =
                response.GetResponseStream();
            var doc = new HtmlDocument();
            doc.Load(responseStream);

            // TODO: Demeter:
            int documentElements =
                doc.DocumentNode.DescendantNodes().Count();

            // TODO: Demeter:
            int htmlLength =
                doc.DocumentNode.InnerHtml.Length;

            // TODO: Demeter:
            int images = doc.DocumentNode.Descendants("img").Count();
            NavigateResult result =
                new NavigateResult(
                statusCode,
                documentElements,
                htmlLength,
                images);
            var links = doc.DocumentNode.Descendants("a");
            foreach (var nextLink in links)
            {
                AddLink(url, result, nextLink);
            }

            return result;
        }

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="url">The parent URL.</param>
        /// <param name="result">The result.</param>
        /// <param name="nextLink">The next link.</param>
        private static void AddLink(
            Uri url,
            NavigateResult result,
            HtmlNode nextLink)
        {
            string uriString = GetValidUriString(nextLink);
            if (string.IsNullOrEmpty(uriString))
            {
                return;
            }

            var rawUrl = new Uri(uriString, UriKind.RelativeOrAbsolute);
            var nextUrl = rawUrl;
            if (!rawUrl.IsAbsoluteUri)
            {
                nextUrl = new Uri(url, rawUrl);
            }

            result.AddLink(new HyperLink(nextUrl, url));
        }

        /// <summary>
        /// Gets the URI string.
        /// </summary>
        /// <param name="nextLink">The next link.</param>
        /// <returns>The uri string.</returns>
        private static string GetValidUriString(HtmlNode nextLink)
        {
            HtmlAttribute hrefAttribute = GetHrefAttribute(nextLink);
            if (hrefAttribute == null)
            {
                return null;
            }

            string uriString = hrefAttribute.Value;
            if (!Uri.IsWellFormedUriString(
                uriString,
                UriKind.RelativeOrAbsolute))
            {
                return null;
            }

            return uriString;
        }

        /// <summary>
        /// Gets the href attribute.
        /// </summary>
        /// <param name="node">The html node.</param>
        /// <returns>The href attribute.</returns>
        private static HtmlAttribute GetHrefAttribute(HtmlNode node)
        {
            if (node == null)
            {
                return null;
            }

            return node.Attributes["href"];
        }
    }
}
