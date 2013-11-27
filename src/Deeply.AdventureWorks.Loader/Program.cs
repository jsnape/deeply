#region Copyright (c) 2013 James Snape
// <copyright file="Program.cs" company="James Snape">
//  Copyright 2013 James Snape
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// </copyright>
#endregion

namespace Deeply.AdventureWorks.Loader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Args;
    using Args.Help.Formatters;
    using Autofac;
    using Autofac.Configuration;
    using Autofac.Extras.CommonServiceLocator;
    using CsvHelper.Configuration;
    using Deeply;
    using Deeply.AdventureWorks.Loader.Domain;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Main program entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Cancellation token source.
        /// </summary>
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Set of connection factories.
        /// </summary>
        private static IDictionary<string, IDbConnectionFactory> connectionFactories;

        /// <summary>
        /// Gets the global options.
        /// </summary>
        public static Options Options { get; private set; }

        /// <summary>
        /// Gets the global container.
        /// </summary>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// Main program entry point.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        internal static void Main(string[] args)
        {
            Console.WriteLine("Parsing arguments");
            Program.Options = ParseArguments(args);

            Console.WriteLine("Registering types");
            Program.Container = RegisterContainer();

            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Cancellation requested, please wait.");
                Program.cancellationTokenSource.Cancel();
            };

            Console.WriteLine("Initializing connection factories");
            Program.connectionFactories = CreateConnectionFactories();

            Console.WriteLine("Building loader sequence");
            var tasks = BuildTasks();

            Console.WriteLine("Executing sequence");
            ExecuteAsync(tasks).Wait();
        }

        /// <summary>
        /// Executes the loader task sequence.
        /// </summary>
        /// <remarks>
        /// Note: This executes the supplied list in sequence. If parallel 
        /// execution is needed then create ParallelTasks in this list.
        /// </remarks>
        /// <param name="tasks">A sequence of tasks to execute.</param>
        /// <returns>A task representing the completion of this function.</returns>
        private static async Task ExecuteAsync(IEnumerable<ITask> tasks)
        {
            var mainTask = new SequentialTask("Main", tasks);

            var context = new TaskContext(Program.cancellationTokenSource);

            await mainTask.VerifyAsync(context);

            await mainTask.ExecuteAsync(context);
        }

        /// <summary>
        /// Builds a set of tasks to perform the load.
        /// </summary>
        /// <returns>A sequence of tasks to execute.</returns>
        private static IEnumerable<ITask> BuildTasks()
        {
            List<ITask> tasks = new List<ITask>();

            var targetFactory = Program.connectionFactories["target"];

            if (Program.Options.FullLoad)
            {
                // Clean out the database.
                var cleanTask = new SequentialTask(
                    "Clean Target Database",
                    new ExecuteSqlTask("Truncate Facts", targetFactory, SqlQueries.TruncateFactsSql),
                    new ExecuteSqlTask("Delete dimension data", targetFactory, SqlQueries.DeleteDimensionData));

                tasks.Add(cleanTask);
            }

            var dimensionTasks = new List<ITask>();

            if (Program.Options.FullLoad)
            {
                var currencyTask = BuildCurrencyLoadTask();
                dimensionTasks.Add(currencyTask);

                var dateTask = BuildDateDimensionLoadTask();
                dimensionTasks.Add(dateTask);
            }

            tasks.Add(new ParallelTask("Load Dimensions", dimensionTasks));

            return tasks;
        }

        /// <summary>
        /// Builds a task to load the currency dimension.
        /// </summary>
        /// <returns>A task for loading the currency dimension.</returns>
        private static ITask BuildCurrencyLoadTask()
        {
            var targetFactory = Program.connectionFactories["target"];

            using (var builder = new SimpleDataflowBuilder<Currency>())
            {
                var currencyFile = Path.Combine(Program.Options.SourcePath, "currency.csv");

                var csvConfiguration = new CsvConfiguration { HasHeaderRecord = false, Delimiter = "\t" };
                csvConfiguration.RegisterClassMap<CurrencyFileMap>();

                return builder
                    .CsvSource(currencyFile, csvConfiguration)
                    .BulkLoad(
                        "dbo.DimCurrency",
                        targetFactory,
                        new Dictionary<string, string>()
                        {
                            { "AlternateKey", "CurrencyAlternateKey" },
                            { "Name", "CurrencyName" }
                        })
                    .Build("Load currency dimension");
            }
        }

        /// <summary>
        /// Builds a task used to load the date dimension.
        /// </summary>
        /// <returns>A task used to load the date dimension.</returns>
        private static ITask BuildDateDimensionLoadTask()
        {
            var targetFactory = Program.connectionFactories["target"];

            var dateTarget = new SqlBulkRepository<DateValue>(
                "dbo.DimDate",
                targetFactory,
                new Dictionary<string, string>()
                {
                    { "Key", "DateKey" },
                    { "Value", "FullDateAlternateKey" },
                    { "DayNumberOfWeek", "DayNumberOfWeek" },
                    { "EnglishDayNameOfWeek", "EnglishDayNameOfWeek" },
                    { "SpanishDayNameOfWeek", "SpanishDayNameOfWeek" },
                    { "FrenchDayNameOfWeek", "FrenchDayNameOfWeek" },
                    { "DayNumberOfMonth", "DayNumberOfMonth" },
                    { "DayNumberOfYear", "DayNumberOfYear" },
                    { "WeekNumberOfYear", "WeekNumberOfYear" },
                    { "EnglishMonthName", "EnglishMonthName" },
                    { "SpanishMonthName", "SpanishMonthName" },
                    { "FrenchMonthName", "FrenchMonthName" },
                    { "MonthNumberOfYear", "MonthNumberOfYear" },
                    { "CalendarQuarter", "CalendarQuarter" },
                    { "CalendarYear", "CalendarYear" },
                    { "CalendarSemester", "CalendarSemester" },
                    { "FiscalQuarter", "FiscalQuarter" },
                    { "FiscalYear", "FiscalYear" },
                    { "FiscalSemester", "FiscalSemester" }
                });

            var dateSource = DateSource.GetDateSequence(new DateTime(2005, 01, 01), new DateTime(2010, 12, 31));

            return new SimpleDataflowTask<DateTime, DateValue>(
                dateSource,
                s => new DateValue(s),
                dateTarget);
        }

        /// <summary>
        /// Creates a set of connection factories.
        /// </summary>
        /// <returns>A mapped set of factories.</returns>
        private static IDictionary<string, IDbConnectionFactory> CreateConnectionFactories()
        {
            var factories = new Dictionary<string, IDbConnectionFactory>();

            factories["target"] = new DbConnectionFactory(
                "Data Source=(local);Initial Catalog=AdventureWorksDW;Integrated Security=SSPI;");

            return factories;
        }

        /// <summary>
        /// Registers the set of types for the global container and service provider.
        /// </summary>
        /// <returns>A pre-configured IOC container.</returns>
        private static IContainer RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DeeplyRegistrationModule>();
            builder.RegisterModule<ConfigurationSettingsReader>();

            var container = builder.Build();

            var serviceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            return container;
        }

        /// <summary>
        /// Parses the command line arguments into 
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        /// <returns>A set of options.</returns>
        private static Options ParseArguments(string[] args)
        {
            var optionModel = Configuration.Configure<Options>();
            var options = optionModel.CreateAndBind(args);

            if (options.Help)
            {
                var help = new Args.Help.HelpProvider().GenerateModelHelp(optionModel);
                var formatter = new ConsoleHelpFormatter();

                Console.WriteLine(formatter.GetHelp(help));
            }

            return options;
        }
    }
}
