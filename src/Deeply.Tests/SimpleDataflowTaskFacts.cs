#region Copyright (c) 2013 James Snape
// <copyright file="SimpleDataflowTaskFacts.cs" company="James Snape">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Deeply.Tests.Fakes;
    using FluentAssertions;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Xunit;

    /// <summary>
    /// SimpleDataflowTask tests.
    /// </summary>
    public class SimpleDataflowTaskFacts
    {
        /// <summary>
        /// Test data fixture.
        /// </summary>
        private readonly Fixture fixture;

        /// <summary>
        /// Test data.
        /// </summary>
        private readonly IEnumerable<FakeEntity> source;

        /// <summary>
        /// Target repository.
        /// </summary>
        private readonly IBulkRepository<FakeEntity> repository;

        /// <summary>
        /// Task Context.
        /// </summary>
        private readonly ITaskContext context;

        /// <summary>
        /// Class under test.
        /// </summary>
        private readonly SimpleDataflowTask<FakeEntity, FakeEntity> dataflow;

        /// <summary>
        /// Generated data source.
        /// </summary>
        private IEnumerable<FakeEntity> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataflowTaskFacts"/> class.
        /// </summary>
        public SimpleDataflowTaskFacts()
        {
            this.fixture = new Fixture();
            this.source = this.fixture.CreateMany<FakeEntity>();
            this.repository = Substitute.For<IBulkRepository<FakeEntity>>();
            this.context = Substitute.For<ITaskContext>();

            this.repository.BulkCopyAsync(
                Arg.Do<IEnumerable<FakeEntity>>(a => this.data = a),
                Arg.Any<ITaskContext>());
            
            this.dataflow = new SimpleDataflowTask<FakeEntity, FakeEntity>(
                this.source,
                MappingFunctions.Identity,
                this.repository);
        }

        /// <summary>
        /// Should send items to the target in order.
        /// </summary>
        /// <returns>A task representing the completion of this function.</returns>
        [Fact]
        public async Task ShouldSendItemsToTargetInOrder()
        {
            await this.dataflow.ExecuteAsync(this.context);

            this.data.Should().ContainInOrder(this.source);
        }

        /// <summary>
        /// Throws then a null source is passed.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.SimpleDataflowTask`2<Deeply.Tests.Fakes.FakeEntity,Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws so no return")]
        [Fact]
        public void ShouldThrowWhenNullSourcePassed()
        {
            Action action = () => new SimpleDataflowTask<FakeEntity, FakeEntity>(
                null,
                MappingFunctions.Identity,
                this.repository);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "source");
        }

        /// <summary>
        /// Throws then a null mapper is passed.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.SimpleDataflowTask`2<Deeply.Tests.Fakes.FakeEntity,Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws so no return")]
        [Fact]
        public void ShouldThrowWhenNullMapperPassed()
        {
            Action action = () => new SimpleDataflowTask<FakeEntity, FakeEntity>(
                this.source,
                null,
                this.repository);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "map");
        }

        /// <summary>
        /// Throws then a null destination is passed.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.SimpleDataflowTask`2<Deeply.Tests.Fakes.FakeEntity,Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws so no return")]
        [Fact]
        public void ShouldThrowWhenNullDestinationPassed()
        {
            Action action = () => new SimpleDataflowTask<FakeEntity, FakeEntity>(
                this.source,
                MappingFunctions.Identity,
                null);

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "destination");
        }
    }
}
