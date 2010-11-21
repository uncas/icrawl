//-------------
// <copyright file="IResultRepository.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories
{
    using System.Diagnostics.CodeAnalysis;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Handles storage of results.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1040:AvoidEmptyInterfaces",
        Justification = "This repository derives from the base repository with a specific type.")]
    public interface IResultRepository : IRepository<HyperLink>
    {
    }
}