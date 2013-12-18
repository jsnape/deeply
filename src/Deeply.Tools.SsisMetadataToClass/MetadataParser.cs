#region Copyright (c) 2013 James Snape
// <copyright file="MetadataParser.cs" company="James Snape">
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

namespace Deeply.Tools.SsisMetadataToClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// MetadataParser class definition.
    /// </summary>
    public class MetadataParser
    {
        /// <summary>
        /// Clipboard column names.
        /// </summary>
        private static readonly string[] ColumnNames = new string[] 
        {
            "Name", "Data Type", "Precision", "Scale", "Length", "Code Page" 
        };

        /// <summary>
        /// Text to parse.
        /// </summary>
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataParser"/> class.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        public MetadataParser(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Argument cannot be an empty string", "text");
            }

            this.text = text;
        }

        /// <summary>
        /// Helper function to setup the parser and parse the supplied text.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>A sequence of <c>SsisMetadata</c> objects</returns>
        /// <exception cref="FormatException">When the text cannot be parsed.</exception>
        public static IEnumerable<SsisMetadata> Parse(string text)
        {
            var parser = new MetadataParser(text);
            return parser.Parse();
        }

        /// <summary>
        /// Parses the supplied text into a sequence of <c>SsisMetadata</c> objects.
        /// </summary>
        /// <returns>A sequence of <c>SsisMetadata</c> objects</returns>
        /// <exception cref="FormatException">When the text cannot be parsed.</exception>
        public IEnumerable<SsisMetadata> Parse()
        {
            var lines = SplitLines(this.text);

            ValidateHeader(lines[0]);

            return lines
                .Skip(1)
                .Select(l => SplitColumns(l))
                .Where(c => c.Length >= ColumnNames.Length)
                .Select(d => ParseDataLine(d));
        }

        /// <summary>
        /// Helper method to parse a line of data and return a <c>SsisMetadata</c> object.
        /// </summary>
        /// <param name="columns">Array of column data.</param>
        /// <returns>A <c>SsisMetadata</c> object</returns>
        /// <exception cref="FormatException">When the text cannot be parsed.</exception>
        private static SsisMetadata ParseDataLine(string[] columns)
        {
            return new SsisMetadata
            {
                Name = columns[0],
                DataType = columns[1],
                Precision = int.Parse(columns[2], CultureInfo.CurrentCulture),
                Scale = int.Parse(columns[3], CultureInfo.CurrentCulture),
                Length = int.Parse(columns[4], CultureInfo.CurrentCulture),
                CodePage = int.Parse(columns[5], CultureInfo.CurrentCulture)
            };
        }

        /// <summary>
        /// Helper method to validate the head contains the correct columns.
        /// </summary>
        /// <param name="header">Header string to split and check.</param>
        private static void ValidateHeader(string header)
        {
            var columns = SplitColumns(header);

            if (columns.Length < ColumnNames.Length)
            {
                throw new FormatException();
            }

            // Check each column name matches the expected name.
            bool invalidColumns =
                columns
                .Zip(ColumnNames, (a, b) => Tuple.Create(a, b)) // Join the two sequences.
                .Where(x => x.Item1 != x.Item2) // Look for any that don't match.
                .Any();

            if (invalidColumns)
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// Helper method to split the text into lines and check that there are at least two.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>An array of lines.</returns>
        private static string[] SplitLines(string text)
        {
            var lines = text.Split('\n');

            if (lines.Length < 2)
            {
                throw new FormatException();
            }

            return lines;
        }

        /// <summary>
        /// Splits a line into columns.
        /// </summary>
        /// <param name="line">Line to split.</param>
        /// <returns>Array of columns.</returns>
        private static string[] SplitColumns(string line)
        {
            return line
                .Replace("\"", string.Empty)
                .Split('\t');
        }
    }
}
