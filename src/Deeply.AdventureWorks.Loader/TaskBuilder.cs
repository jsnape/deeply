#region Copyright (c) 2013 James Snape
// <copyright file="TaskBuilder.cs" company="James Snape">
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
    
    /// <summary>
    /// TaskBuilder class definition.
    /// </summary>
    /// <typeparam name="T">Type of entity being loaded.</typeparam>
    public class TaskBuilder<T> : IDisposable
    {
        /// <summary>
        /// Set of resources to automatically release in case of exception.
        /// </summary>
        private HashSet<IDisposable> resources = new HashSet<IDisposable>();

        /// <summary>
        /// Source data.
        /// </summary>
        private IEnumerable<T> source;

        /// <summary>
        /// Bulk load target.
        /// </summary>
        private SqlBulkRepository<T> target;

        /// <summary>
        /// Finalizes an instance of the <see cref="TaskBuilder{T}"/> class.
        /// </summary>
        ~TaskBuilder()
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
        /// Builds the final task and returns it.
        /// </summary>
        /// <param name="taskName">Name for the new task.</param>
        /// <returns>A built and configured task.</returns>
        public ITask Build(string taskName)
        {
            var task = new SimpleDataflowTask<T, T>(taskName, this.source, MappingFunctions.Identity, this.target);

            var fileScope = new ResourceScope(task, this.resources);

            // Ensure that these resource are disposed of one way xor another.
            this.resources = new HashSet<IDisposable>();
                
            return fileScope;
        }

        /// <summary>
        /// Sets up a source file.
        /// </summary>
        /// <param name="path">File path of the file.</param>
        /// <param name="configuration">File settings.</param>
        /// <returns>This <c>TaskBuilder</c> instance for fluent calls.</returns>
        [CLSCompliant(false)]
        public TaskBuilder<T> CsvSource(string path, CsvConfiguration configuration)
        {
            var reader = new StreamReader(path);
            this.resources.Add(reader);

            var csvSource = new CsvReader(reader, configuration);
            this.resources.Add(csvSource);

            this.source = csvSource.GetRecords<T>();

            return this;
        }

        /// <summary>
        /// Sets up the target repository.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="columnMappings">The column mappings.</param>
        /// <returns>This <c>TaskBuilder</c> instance for fluent calls.</returns>
        public TaskBuilder<T> BulkLoad(string tableName, IDbConnectionFactory connectionFactory, IDictionary<string, string> columnMappings)
        {
            this.target = new SqlBulkRepository<T>(tableName, connectionFactory, columnMappings);

            return this;
        }

        /// <summary>
        /// Called to release any resources held by this instance.
        /// </summary>
        /// <param name="disposing">Set to <c>true</c> when disposing, <c>false</c> when finalizing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var resource in this.resources)
                {
                    resource.Dispose();
                }

                this.resources.Clear();
            }
        }
    }
}
