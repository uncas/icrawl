//-------------
// <copyright file="LogEntry.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Models
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Holds info about a log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public LogEntry(Exception exception, string message)
            : this(exception)
        {
            this.Message = string.Format(
                CultureInfo.CurrentCulture,
                "{0} |{1} {2}",
                message,
                Environment.NewLine,
                exception != null ? exception.Message : null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <exception cref="System.ArgumentNullException">The exception parameter is null.</exception>
        public LogEntry(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(
                    "exception",
                    "Exception should not be null");
            }

            this.ExceptionType = exception.GetType().ToString();
            this.StackTrace = exception.StackTrace;
            this.Message = exception.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LogEntry(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public static string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        public string ExceptionType { get; private set; }

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        public string StackTrace { get; private set; }
    }
}
