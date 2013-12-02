#region Copyright (c) 2013 James Snape
// <copyright file="CurrencyLoader.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader.Workflow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper.Configuration;
    using Deeply.AdventureWorks.Loader.Domain;

    /// <summary>
    /// Currency loader class definition.
    /// </summary>
    public class CurrencyLoader
    {
        /// <summary>
        /// Target factory.
        /// </summary>
        private readonly IDbConnectionFactory targetFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyLoader"/> class.
        /// </summary>
        /// <param name="targetFactory">Target connection factory.</param>
        public CurrencyLoader(IDbConnectionFactory targetFactory)
        {
            if (targetFactory == null)
            {
                throw new ArgumentNullException("targetFactory");
            }

            this.targetFactory = targetFactory;
        }

        /// <summary>
        /// Builds a task to load the currency dimension.
        /// </summary>
        /// <returns>A task for loading the currency dimension.</returns>
        public ITask Build()
        {
            using (var builder = new SimpleDataflowBuilder<Currency>())
            {
                var currencyFile = Path.Combine(Program.Options.SourcePath, "currency.csv");

                var csvConfiguration = new CsvConfiguration { HasHeaderRecord = false, Delimiter = "\t" };
                csvConfiguration.RegisterClassMap<CurrencyFileMap>();

                return builder
                    .CsvSource(currencyFile, csvConfiguration)
                    .BulkLoad(
                        "dbo.DimCurrency",
                        this.targetFactory,
                        new Dictionary<string, string>()
                        {
                            { "AlternateKey", "CurrencyAlternateKey" },
                            { "Name", "CurrencyName" }
                        })
                    .Build("Load currency dimension");
            }
        }
    }
}
