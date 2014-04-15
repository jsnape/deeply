#region Copyright (c) 2013 James Snape
// <copyright file="CsvBulkRepository.cs" company="James Snape">
//  Copyright 2014 James Snape
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

namespace Deeply.Extras
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Deeply;

    /// <summary>
    /// CSV bulk repository class definition.
    /// </summary>
    /// <typeparam name="T">Entity type to be bulk inserted.</typeparam>
    public class CsvBulkRepository<T> : IBulkRepository<T>
    {
        /// <summary>
        /// The file name
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// The CSV configuration
        /// </summary>
        private readonly CsvConfiguration configuration;

        /// <summary>
        /// The create writer function.
        /// </summary>
        private readonly Func<string, TextWriter> createWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvBulkRepository{T}"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public CsvBulkRepository(string fileName)
            : this(fileName, new CsvConfiguration())
        {                    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvBulkRepository{T}" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="configuration">The CSV writer configuration.</param>
        [CLSCompliant(false)]
        public CsvBulkRepository(string fileName, CsvConfiguration configuration)
            : this(fileName, configuration, f => new StreamWriter(f))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvBulkRepository{T}" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="configuration">The CSV writer configuration.</param>
        /// <param name="createWriter">The writer creation function.</param>
        [CLSCompliant(false)]
        public CsvBulkRepository(string fileName, CsvConfiguration configuration, Func<string, TextWriter> createWriter)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (createWriter == null)
            {
                throw new ArgumentNullException("createWriter");
            }

            this.fileName = fileName;
            this.configuration = configuration;
            this.createWriter = createWriter;
        }

        /// <summary>
        /// Bulk copies the supplied data into the database.
        /// </summary>
        /// <param name="rows">A row iterator</param>
        /// <param name="context">Task execution context</param>
        /// <returns>A task representing the completion of this function.</returns>
        public async Task BulkCopyAsync(IEnumerable<T> rows, ITaskContext context)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // No need to dispose of this since this next line will fail or the
            // CsvWriter will own the TextWriter and assume responsibility of
            // disposal.
            var fileWriter = this.createWriter(this.fileName);

            using (var csvWriter = new CsvWriter(fileWriter, this.configuration))
            {
                await Task.Run(() => csvWriter.WriteRecords((IEnumerable)rows));
            }
        }
    }
}
