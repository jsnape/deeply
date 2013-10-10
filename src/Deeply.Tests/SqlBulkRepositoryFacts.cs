#region Copyright (c) 2013 James Snape
// <copyright file="SqlBulkRepositoryFacts.cs" company="James Snape">
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

namespace Deeply.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Deeply.Internals;
    using Deeply.Tests.Fakes;
    using FluentAssertions;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Xunit;

    /// <summary>
    /// <c>SqlBulkRepository</c> tests.
    /// </summary>
    public class SqlBulkRepositoryFacts
    {
        /// <summary>
        /// Test table name.
        /// </summary>
        private const string TestTableName = "dbo.TestTableName";

        /// <summary>
        /// Test data fixture.
        /// </summary>
        private readonly Fixture fixture;

        /// <summary>
        /// Fake bulk copy implementation.
        /// </summary>
        private readonly IBulkCopy bulkCopy;

        /// <summary>
        /// Fake connection factory implementation.
        /// </summary>
        private readonly IDbConnectionFactory connectionFactory;

        /// <summary>
        /// Column mapping collection.
        /// </summary>
        private readonly IDictionary<string, string> columnMappings;

        /// <summary>
        /// Test data to bulk copy.
        /// </summary>
        private readonly IEnumerable<FakeEntity> data;

        /// <summary>
        /// Fake task context.
        /// </summary>
        private readonly ITaskContext context;

        /// <summary>
        /// Class under test.
        /// </summary>
        private readonly SqlBulkRepository<FakeEntity> bulkRepository;

        /// <summary>
        /// Initializes a new instance of the  <see cref="SqlBulkRepositoryFacts"/> class.
        /// </summary>
        public SqlBulkRepositoryFacts()
        {
            this.fixture = new Fixture();

            this.bulkCopy = Substitute.For<IBulkCopy>();
            this.connectionFactory = Substitute.For<IDbConnectionFactory>();

            this.columnMappings = new Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "Name", "Name" },
                { "Created", "Created" }
            };

            this.bulkRepository = new SqlBulkRepository<FakeEntity>(
                (c, o) => this.bulkCopy,
                TestTableName,
                this.connectionFactory,
                this.columnMappings);

            this.data = this.fixture.CreateMany<FakeEntity>();
            this.context = Substitute.For<ITaskContext>();
        }

        /// <summary>
        /// Should pass the correct table name to the bulk copy implementation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ShouldPassCorrectTableName()
        {
            await this.bulkRepository.BulkCopyAsync(this.data, this.context);

            this.bulkCopy.Received().DestinationTableName = TestTableName;
        }

        /// <summary>
        /// Should pass data to bulk copy implementation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ShouldPassDataToBulkCopyImplementation()
        {
            int count = 0;

            await this.bulkCopy.WriteToServerAsync(
                Arg.Do<IDataReader>(x => count = CountReaderItems(x)),
                Arg.Any<CancellationToken>());

            await this.bulkRepository.BulkCopyAsync(this.data, this.context);

            count.Should().Be(this.data.Count());
        }

        /// <summary>
        /// Should pass the correct column mappings.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ShouldPassCorrectColumnMappings()
        {
            await this.bulkRepository.BulkCopyAsync(this.data, this.context);

            foreach (var key in this.columnMappings.Keys)
            {
                this.bulkCopy.Received(1).AddColumnMapping(key, this.columnMappings[key]);
            }
        }

        /// <summary>
        /// Throws when a null reader is passed.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task ThrowsWhenNullReaderPassed()
        {
            try
            {
                await this.bulkRepository.BulkCopyAsync(null, this.context);
                Assert.True(false);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("rows");
            }
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
                await this.bulkRepository.BulkCopyAsync(this.data, null);
                Assert.True(false);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("context");
            }
        }

        /// <summary>
        /// Default constructor does not throw.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.SqlBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Constructor throws so no object returned.")]
        [Fact]
        public void DefaultConstructorDoesNotThrow()
        {
            Action action = () => new SqlBulkRepository<FakeEntity>(
                TestTableName,
                this.connectionFactory,
                this.columnMappings);

            action.ShouldNotThrow();
        }

        /// <summary>
        /// Invalid bulk copy create function should throw.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.SqlBulkRepository`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Constructor throws so no object returned.")]
        [Fact]
        public void InvalidCreateFunctionThrows()
        {
            Action action = () => new SqlBulkRepository<FakeEntity>(
                null,
                TestTableName,
                this.connectionFactory,
                this.columnMappings);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "createBulkCopy");
        }

        /// <summary>
        /// Helper function to count the number of items in a data reader.
        /// </summary>
        /// <param name="reader">Reader to iterate.</param>
        /// <returns>The number of rows in the reader.</returns>
        private static int CountReaderItems(IDataReader reader)
        {
            int count = 0;

            while (reader.Read())
            {
                count++;
            }

            return count;
        }
    }
}
