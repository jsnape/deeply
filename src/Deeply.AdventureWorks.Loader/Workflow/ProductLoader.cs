#region Copyright (c) 2013 James Snape
// <copyright file="ProductLoader.cs" company="James Snape">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using CsvHelper.Configuration;
    using Deeply.AdventureWorks.Loader.Domain;

    /// <summary>
    /// ProductLoader class definition.
    /// </summary>
    public class ProductLoader : IDisposable
    {
        /// <summary>
        /// Target factory.
        /// </summary>
        private readonly IDbConnectionFactory targetFactory;

        /// <summary>
        /// Surrogate key mapper for product categories.
        /// </summary>
        private KeyMapper<int> productCategoryMapper;

        /// <summary>
        /// Surrogate key mapper for product subcategories.
        /// </summary>
        private KeyMapper<int> productSubcategoryMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductLoader"/> class.
        /// </summary>
        /// <param name="targetFactory">Target connection factory.</param>
        public ProductLoader(IDbConnectionFactory targetFactory)
        {
            if (targetFactory == null)
            {
                throw new ArgumentNullException("targetFactory");
            }

            this.targetFactory = targetFactory;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProductLoader"/> class.
        /// </summary>
        ~ProductLoader()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Called to release any resources held by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Builds a task used to load the product dimension.
        /// </summary>
        /// <returns>A task used to load the product dimension.</returns>
        public ITask Build()
        {
            return new SequentialTask(
                "Product Sequence",
                this.BuildProductCategoryTask(),
                new ActionTask(
                    "Prime product category key mapper",
                    () => this.PrimeProductCategoryKeyMapper()),
                this.BuildProductSubcategoryTask(),
                new ActionTask(
                    "Prime product subcategory key mapper",
                    () => this.PrimeProductSubcategoryKeyMapper()),
                this.BuildProductTask());
        }

        /// <summary>
        /// Called to release any resources held by this instance.
        /// </summary>
        /// <param name="disposing">Set to <c>true</c> when disposing, <c>false</c> when finalizing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.productSubcategoryMapper != null)
                {
                    this.productSubcategoryMapper.Dispose();
                }

                if (this.productCategoryMapper != null)
                {
                    this.productCategoryMapper.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads the set of product categories from the database
        /// for surrogate key mapping.
        /// </summary>
        private void PrimeProductCategoryKeyMapper()
        {
            var initialCache = this.LoadKeyCache(
                SqlQueries.ProductCategoryKeyMap,
                "ProductCategoryKey",
                "BusinessKey");

            this.productCategoryMapper = new KeyMapper<int>(
                initialCache,
                x => 
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture, 
                        "Error, product category key '{0}' not found in cache", 
                        x);

                    throw new InvalidOperationException(message);
                });
        }

        /// <summary>
        /// Loads the set of product subcategories from the database
        /// for surrogate key mapping.
        /// </summary>
        private void PrimeProductSubcategoryKeyMapper()
        {
            var initialCache = this.LoadKeyCache(
                SqlQueries.ProductSubcategoryKeyMap,
                "ProductSubcategoryKey",
                "BusinessKey");

            this.productSubcategoryMapper = new KeyMapper<int>(
                initialCache,
                x =>
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        "Error, product subcategory key '{0}' not found in cache",
                        x);

                    throw new InvalidOperationException(message);
                });
        }

        /// <summary>
        /// Loads a dictionary with a cache of keys from the supplied query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <param name="surrogateKeyName">The name of the surrogate key column.</param>
        /// <param name="businessKeyColumn">The name of the business key column.</param>
        /// <returns>A <c>Dictionary</c> of keys.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Reviewed, no user input in SQL string.")]
        private Dictionary<string, int> LoadKeyCache(string query, string surrogateKeyName, string businessKeyColumn)
        {
            var initialCache = new Dictionary<string, int>();

            using (var connection = this.targetFactory.CreateConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    int keyOrdinal = reader.GetOrdinal(surrogateKeyName);
                    int businessKeyOrdinal = reader.GetOrdinal(businessKeyColumn);

                    while (reader.Read())
                    {
                        int surrogateKey = reader.GetInt32(keyOrdinal);
                        string businessKey = reader.GetString(businessKeyOrdinal);

                        initialCache[businessKey] = surrogateKey;
                    }
                }
            }

            return initialCache;
        }

        /// <summary>
        /// Builds the product category loader tasks.
        /// </summary>
        /// <returns>A task used to load the product category table.</returns>
        private ITask BuildProductCategoryTask()
        {
            using (var builder = new SimpleDataflowBuilder<ProductCategory>())
            {
                var categoryFile = Path.Combine(Program.Options.SourcePath, "ProductCategory.csv");

                var csvConfiguration = new CsvConfiguration { HasHeaderRecord = false, Delimiter = "\t" };
                csvConfiguration.RegisterClassMap<ProductCategoryFileMap>();

                return builder
                    .CsvSource(categoryFile, csvConfiguration)
                    .BulkLoad(
                        "dbo.DimProductCategory",
                        this.targetFactory,
                        new Dictionary<string, string>()
                        {
                            { "AlternateKey", "ProductCategoryAlternateKey" },
                            { "Name", "EnglishProductCategoryName" },
                            { "SpanishName", "SpanishProductCategoryName" },
                            { "FrenchName", "FrenchProductCategoryName" }
                        })
                    .Build("Load product category dimension");
            }
        }

        /// <summary>
        /// Builds the product subcategory loader tasks.
        /// </summary>
        /// <returns>A task used to load the product subcategory table.</returns>
        private ITask BuildProductSubcategoryTask()
        {
            using (var builder = new SimpleDataflowBuilder<ProductSubcategory>())
            {
                var categoryFile = Path.Combine(Program.Options.SourcePath, "ProductSubcategory.csv");

                var csvConfiguration = new CsvConfiguration { HasHeaderRecord = false, Delimiter = "\t" };
                csvConfiguration.RegisterClassMap<ProductSubcategoryFileMap>();

                return builder
                    .CsvSource(categoryFile, csvConfiguration)
                    .Map(x => 
                    {
                        x.ProductCategoryKey = 
                            this.productCategoryMapper.Map(
                                x.ProductCategoryAlternateKey.ToString(CultureInfo.CurrentCulture));
                    
                        return x;
                    })
                    .BulkLoad(
                        "dbo.DimProductSubcategory",
                        this.targetFactory,
                        new Dictionary<string, string>()
                        {
                            { "ProductCategoryKey", "ProductCategoryKey" },
                            { "AlternateKey", "ProductSubcategoryAlternateKey" },
                            { "Name", "EnglishProductSubcategoryName" },
                            { "SpanishName", "SpanishProductSubcategoryName" },
                            { "FrenchName", "FrenchProductSubcategoryName" }
                        })
                    .Build("Load product subcategory dimension");
            }
        }

        /// <summary>
        /// Builds the product loader tasks.
        /// </summary>
        /// <returns>A task used to load the product subcategory table.</returns>
        private ITask BuildProductTask()
        {
            using (var builder = new SimpleDataflowBuilder<Product>())
            {
                var productFile = Path.Combine(Program.Options.SourcePath, "Product.csv");

                var csvConfiguration = new CsvConfiguration { HasHeaderRecord = false, Delimiter = "\t" };
                csvConfiguration.RegisterClassMap<ProductFileMap>();

                return builder
                    .CsvSource(productFile, csvConfiguration)
                    .Map(x =>
                    {
                        x.ProductSubcategoryKey =
                            this.productSubcategoryMapper.Map(
                                x.ProductSubcategoryAlternateKey.ToString(CultureInfo.CurrentCulture));

                        return x;
                    })
                    .BulkLoad(
                        "dbo.DimProduct",
                        this.targetFactory,
                        new Dictionary<string, string>()
                        {
                            { "ProductSubcategoryKey", "ProductSubcategoryKey" },
                            { "AlternateKey", "ProductAlternateKey" },
                            { "Name", "EnglishProductName" },
                        })
                    .Build("Load product dimension");
            }
        }
    }
}
