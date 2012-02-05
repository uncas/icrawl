//-------------
// <copyright file="Program.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Infrastructure;
    using Uncas.WebTester.Repositories;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// The console application for the web tester.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        private readonly IList<string> commandLineArguments;

        /// <summary>
        /// The IoC container.
        /// </summary>
        private IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="commandLineArguments">The command line arguments.</param>
        public Program(IList<string> commandLineArguments)
        {
            this.commandLineArguments = commandLineArguments;
        }

        /// <summary>
        /// The main entry point for the console application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        [STAThread]
        internal static void Main(string[] args)
        {
            Program program = new Program(args);
            program.Run();
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        private void Run()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(
                    this.CurrentDomain_UnhandledException);
            this.ConfigureContainer();
            string webTestType = CombinationParser.GetValue(
                this.commandLineArguments,
                "type",
                "type",
                "crawl");
            if (webTestType == "list")
            {
                IUrlListCheckerService urlListChecker =
                    this.container.Resolve<IUrlListCheckerService>();
                IUrlListProvider listProvider =
                    this.container.Resolve<IUrlListProvider>();
                Console.WriteLine("Checking a list of urls...\n");
                urlListChecker.CheckUrlList(listProvider);
            }
            else
            {
                ICrawlerService crawlerService =
                    this.container.Resolve<ICrawlerService>();
                ICrawlConfiguration configuration =
                    CrawlConfigurationParser.ParseArguments(this.commandLineArguments);
                Console.WriteLine("Crawling...\n {0}", configuration);
                crawlerService.Crawl(configuration);
            }

            this.DisposeBrowserUtility();
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            ILoggerService logger = this.container.Resolve<ILoggerService>();
            var exception = (Exception)e.ExceptionObject;
            logger.LogError(
                exception,
                "Unhandled exception");
            Console.WriteLine(exception);
            Console.WriteLine("Exiting... An error occurred. See details above.");
            Environment.Exit(1);
        }

        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        private void ConfigureContainer()
        {
            const string ConnectionStringDefault =
@"Server=.\SqlExpress;Database=WebTester;Integrated Security=true;";
            Console.WriteLine(
                "Usage: -url http://www.google.com -type crawl (or -type list) -maxPages 10 -connectionString \"" +
                ConnectionStringDefault + "\"");
            string connectionString = this.GetConnectionString(
                ConnectionStringDefault);
            this.container = Bootstrapper.Configure(connectionString);
        }

        /// <summary>
        /// Disposes the browser utility.
        /// </summary>
        private void DisposeBrowserUtility()
        {
            IBrowserUtility browserUtility = 
                this.container.Resolve<IBrowserUtility>();
            IDisposable disposableBrowserUtility = 
                browserUtility as IDisposable;
            if (disposableBrowserUtility != null)
            {
                disposableBrowserUtility.Dispose();
            }
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The connection string.</returns>
        private string GetConnectionString(
            string defaultValue)
        {
            string value =
                CommandLineParser.GetCommandLineValue(
                this.commandLineArguments,
                "connectionString");
            if (string.IsNullOrEmpty(value))
            {
                value = ConfigFileParser.GetConnectionStringFromConfigFile(
                    "webTesterConnectionString", defaultValue);
            }

            return value;
        }
    }
}