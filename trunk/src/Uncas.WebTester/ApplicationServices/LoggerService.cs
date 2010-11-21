//-------------
// <copyright file="LoggerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories;

    /// <summary>
    /// Handles exceptions and other logging.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        /// <summary>
        /// The log entry repository.
        /// </summary>
        private readonly ILogEntryRepository logEntryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerService"/> class.
        /// </summary>
        /// <param name="logEntryRepository">The log entry repository.</param>
        public LoggerService(ILogEntryRepository logEntryRepository)
        {
            this.logEntryRepository = logEntryRepository;
        }

        /// <summary>
        /// Logs the exception as an error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public void LogError(Exception exception, string message)
        {
            var logEntry = new LogEntry(exception, message);
            this.AddLogEntry(logEntry);
        }

        /// <summary>
        /// Logs the exception as an error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void LogError(Exception exception)
        {
            var logEntry = new LogEntry(exception);
            this.AddLogEntry(logEntry);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            var logEntry = new LogEntry(message);
            this.AddLogEntry(logEntry);
        }

        /// <summary>
        /// Adds the log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        private void AddLogEntry(LogEntry logEntry)
        {
            this.logEntryRepository.Add(logEntry);
        }
    }
}