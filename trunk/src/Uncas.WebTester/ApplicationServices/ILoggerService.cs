//-------------
// <copyright file="ILoggerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;

    /// <summary>
    /// Handles exceptions and other logging.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs the exception as an error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void LogError(Exception exception);

        /// <summary>
        /// Logs the exception as an error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        void LogError(Exception exception, string message);

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogError(string message);
    }
}
