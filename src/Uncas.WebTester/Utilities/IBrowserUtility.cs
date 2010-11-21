//-------------
// <copyright file="IBrowserUtility.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Utilities
{
    using System;

    /// <summary>
    /// Represents web browser functionality.
    /// </summary>
    public interface IBrowserUtility
    {
        /// <summary>
        /// Navigates to the given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <returns>The navigate result.</returns>
        NavigateResult NavigateTo(Uri url);
    }
}
