#region Copyright (c) 2013 James Snape
// <copyright file="CsvBulkRepositoryFacts.cs" company="James Snape">
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

namespace Deeply.Extras.Tests
{
    using System;
    using System.IO;
    using CsvHelper;
    using Xunit;

    /// <summary>
    /// CsvBulkRepository facts
    /// </summary>
    public static class CsvBulkRepositoryFacts
    {
        /// <summary>
        /// The CSV reader should dispose of the text reader it is passed.
        /// </summary>
        [Fact]
        public static void CsvReaderShouldDisposeOfReader()
        {
            var csvData = @"COL1,COL2
Foo,Bar";

            var reader = new StringReader(csvData);

            try
            {
                using (var csv = new CsvReader(reader))
                {
                    reader = null;
                }
            }
            catch (Exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }

                throw;
            }
        }
    }
}
