//-------------
// <copyright file="IUrlListProvider.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories
{
    using System.Diagnostics.CodeAnalysis;
    using Uncas.WebTester.Models;

    /// <summary>
    /// The url list provider.
    /// </summary>
    public interface IUrlListProvider
    {
        /// <summary>
        /// Gets the next nextLink.
        /// </summary>
        /// <returns>The next nextLink.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This might involve a resource like a database.")]
        HyperLink GetNextLink();
    }
}