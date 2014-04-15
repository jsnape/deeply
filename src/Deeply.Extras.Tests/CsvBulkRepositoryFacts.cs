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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using CsvHelper.Configuration;
    using Deeply.Tests.Fakes;
    using FluentAssertions;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Xunit;

    /// <summary>
    /// CsvBulkRepository facts
    /// </summary>
    public sealed class CsvBulkRepositoryFacts : IDisposable
    {
        /// <summary>
        /// Test file name.
        /// </summary>
        private const string TestFileName = "ATestFile.csv";

        /// <summary>
        /// Test data fixture.
        /// </summary>
        private readonly Fixture fixture;

        /// <summary>
        /// Test data to bulk copy.
        /// </summary>
        private readonly IEnumerable<FakeEntity> data;

        /// <summary>
        /// Text writer for CSV to be written to.
        /// </summary>
        private readonly TextWriter outputWriter;

        /// <summary>
        /// Fake task context.
        /// </summary>
        private readonly ITaskContext context;

        /// <summary>
        /// Class under test.
        /// </summary>
        private readonly CsvBulkRepository<FakeEntity> target;
        
        /// <summary>
        /// Initializes a new instance of the  <see cref="CsvBulkRepositoryFacts"/> class.
        /// </summary>
        public CsvBulkRepositoryFacts()
        {
            this.fixture = new Fixture();

            this.outputWriter = new StringWriter(CultureInfo.CurrentCulture);

            this.target = new CsvBulkRepository<FakeEntity>(
                TestFileName, 
                new CsvConfiguration(),
                f => this.outputWriter);

            this.data = this.fixture.CreateMany<FakeEntity>();
            this.context = Substitute.For<ITaskContext>();
        }

        /// <summary>
        /// A null constructor file name should throw an exception.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.Extras.CsvBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws an exception")]
        [Fact]
        public static void NullFileNameShouldThrow()
        {
            Action action = () => new CsvBulkRepository<FakeEntity>(null);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "fileName");
        }

        /// <summary>
        /// A blank constructor file name should throw an exception.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.Extras.CsvBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws an exception")]
        [Fact]
        public static void BlankFileNameShouldThrow()
        {
            Action action = () => new CsvBulkRepository<FakeEntity>(string.Empty);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "fileName");
        }

        /// <summary>
        /// A null constructor configuration should throw an exception.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.Extras.CsvBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws an exception")]
        [Fact]
        public static void NullConfigurationShouldThrow()
        {
            Action action = () => new CsvBulkRepository<FakeEntity>(TestFileName, null);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "configuration");
        }

        /// <summary>
        /// A null constructor configuration should throw an exception.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.Extras.CsvBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws an exception")]
        [Fact]
        public static void NullWriterFunctionShouldThrow()
        {
            Action action = () => new CsvBulkRepository<FakeEntity>(TestFileName, new CsvConfiguration(), null);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "createWriter");
        }

        /// <summary>
        /// Called to dispose of any resources this class holds.
        /// </summary>
        public void Dispose()
        {
            if (this.outputWriter != null)
            {
                this.outputWriter.Close();
            }
        }

        /// <summary>
        /// Should pass data to bulk copy implementation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ShouldPassDataToWriter()
        {
            await this.target.BulkCopyAsync(this.data, this.context);

            var results = this.outputWriter.ToString();

            results.Length.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Throws when a null context is passed.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ThrowsWhenNullContextPassed()
        {
            try
            {
                await this.target.BulkCopyAsync(this.data, null);
                Assert.True(false);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("context");
            }
        }

        /// <summary>
        /// Throws when a null context is passed.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ThrowsWhenNullDataPassed()
        {
            try
            {
                await this.target.BulkCopyAsync(null, this.context);
                Assert.True(false);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("rows");
            }
        }
    }
}
