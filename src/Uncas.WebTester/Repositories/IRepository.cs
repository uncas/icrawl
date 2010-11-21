//-------------
// <copyright file="IRepository.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories
{
    /// <summary>
    /// Represents a generic repository.
    /// </summary>
    /// <typeparam name="T">The type of the item to store.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds the specified item to the repository.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(T item);
    }
}