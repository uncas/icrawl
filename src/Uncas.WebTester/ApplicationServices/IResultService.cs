//-------------
// <copyright file="IResultService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using Uncas.WebTester.Models;

    /// <summary>
    /// Handles results.
    /// </summary>
    public interface IResultService
    {
        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        void ProcessResult(HyperLink link);
    }
}
