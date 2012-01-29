//-------------
// <copyright file="NavigateHelper.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Helper for navigation.
    /// </summary>
    internal static class NavigateHelper
    {
        /// <summary>
        /// Navigates to and process URL.
        /// </summary>
        /// <param name="url">The URL to process.</param>
        /// <param name="browserUtility">The browser utility.</param>
        /// <returns>
        /// The navigate result.
        /// </returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This is relatively high in the app...")]
        public static NavigateResult NavigateToAndProcessUrl(
            Uri url,
            IBrowserUtility browserUtility)
        {
            NavigateResult result;
            try
            {
                result = browserUtility.NavigateTo(url);
            }
            catch
            {
                result = null;
            }

            if (result == null)
            {
                result = NavigateResult.Empty;
            }

            return result;
        }
    }
}
