//-------------
// <copyright file="ConfigFileParser.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Infrastructure
{
    using System.Configuration;

    /// <summary>
    /// Parses config file.
    /// </summary>
    public static class ConfigFileParser
    {
        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="appSettingName">Name of the app setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The config value.</returns>
        public static string GetConfigValue(
            string appSettingName,
            string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[appSettingName];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Gets the int32 value.
        /// </summary>
        /// <param name="appSettingName">Name of the app setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The config value.</returns>
        public static int GetInt32Value(
            string appSettingName,
            int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[appSettingName];
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the connection string from the config file.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The connection string.</returns>
        public static string GetConnectionStringFromConfigFile(
            string connectionStringName,
            string defaultValue)
        {
            ConnectionStringSettings connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionString != null)
            {
                return connectionString.ConnectionString;
            }

            return defaultValue;
        }
    }
}