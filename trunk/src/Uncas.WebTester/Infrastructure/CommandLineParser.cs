//-------------
// <copyright file="CommandLineParser.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// Parser of command line arguments.
    /// </summary>
    public static class CommandLineParser
    {
        /// <summary>
        /// Gets the command line value.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="name">The name of the value.</param>
        /// <returns>The requested value.</returns>
        public static string GetCommandLineValue(
            IList<string> args,
            string name)
        {
            string namePart = "-" + name;
            if (!IsArgsContaining(args, namePart))
            {
                return null;
            }

            int valueIndex = args.IndexOf(namePart) + 1;
            if (valueIndex < args.Count)
            {
                return args[valueIndex];
            }

            return null;
        }

        /// <summary>
        /// Determines whether the args contain the specified name part.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="namePart">The name part.</param>
        /// <returns>
        /// <c>true</c> if the args contain the name part; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsArgsContaining(
            IList<string> args,
            string namePart)
        {
            if (args == null)
            {
                return false;
            }

            return args.Contains(namePart);
        }
    }
}