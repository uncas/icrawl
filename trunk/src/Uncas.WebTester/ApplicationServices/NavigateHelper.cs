//-------------
// <copyright file="NavigateHelper.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System.Diagnostics.CodeAnalysis;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Helper for navigation.
    /// </summary>
    internal static class NavigateHelper
    {
        /// <summary>
        /// Navigates to and process nextLink.
        /// </summary>
        /// <param name="browserUtility">The browser utility.</param>
        /// <param name="resultService">The result service.</param>
        /// <param name="nextLink">The next nextLink.</param>
        /// <returns>The navigate result.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This is relatively high in the app...")]
        public static NavigateResult NavigateToAndProcessLink(
                    IBrowserUtility browserUtility,
                    IResultService resultService,
                    HyperLink nextLink)
        {
            NavigateResult result;
            try
            {
                result = browserUtility.NavigateTo(nextLink.Url);
            }
            catch
            {
                result = null;
            }

            if (result == null)
            {
                result = NavigateResult.Empty;
            }

            nextLink.UpdateWithNavigateResult(
                result.LoadTime,
                result.StatusCode,
                result.NumberOfLinks,
                result.Images,
                result.HtmlLength,
                result.DocumentElements);
            resultService.ProcessResult(nextLink);
            return result;
        }
    }
}
