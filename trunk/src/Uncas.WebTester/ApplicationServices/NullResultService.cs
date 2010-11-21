//-------------
// <copyright file="NullResultService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using Uncas.WebTester.Models;

    /// <summary>
    /// A result service that does nothing.
    /// </summary>
    public class NullResultService : IResultService
    {
        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
        }
    }
}
