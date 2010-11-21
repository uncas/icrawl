//-------------
// <copyright file="Bootstrapper.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using Autofac;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Repositories;
    using Uncas.WebTester.Repositories.Sql;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Bootstrapper for the console application.
    /// </summary>
    internal static class Bootstrapper
    {
        /// <summary>
        /// The unity container.
        /// </summary>
        private static ContainerBuilder builder = new ContainerBuilder();

        /// <summary>
        /// Configures the console application..
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The unity container.</returns>
        internal static IContainer Configure(string connectionString)
        {
            builder.Register(c => new CombinationResultService(connectionString)).As<IResultService>();
            builder.RegisterType<HtmlAgilityBrowserUtility>().As<IBrowserUtility>();
            builder.RegisterType<UrlListCheckerService>().As<IUrlListCheckerService>();
            builder.Register(c => new SqlUrlListProvider(connectionString)).As<IUrlListProvider>();
            builder.RegisterType<CrawlerService>().As<ICrawlerService>();
            builder.Register(c => new SqlLogEntryRepository(connectionString)).As<ILogEntryRepository>();
            builder.RegisterType<LoggerService>().As<ILoggerService>();

            return builder.Build();
        }
    }
}