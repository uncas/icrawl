//-------------
// <copyright file="CombinationParser.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using System.Collections.Generic;
    using Uncas.WebTester.Infrastructure;

    /// <summary>
    /// Parser that combines command line and config file.
    /// </summary>
    public static class CombinationParser
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="commandLineName">Name of the command line.</param>
        /// <param name="appSettingName">Name of the app setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The requested value.</returns>
        public static string GetValue(
            IList<string> args,
            string commandLineName,
            string appSettingName,
            string defaultValue)
        {
            string value =
                CommandLineParser.GetCommandLineValue(
                args,
                commandLineName);
            if (value == null)
            {
                value = ConfigFileParser.GetConfigValue(appSettingName, defaultValue);
            }

            return value;
        }
    }
}
