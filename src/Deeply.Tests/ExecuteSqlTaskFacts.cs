#region Copyright (c) 2013 James Snape
// <copyright file="ExecuteSqlTaskFacts.cs" company="James Snape">
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
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Deeply.Tests.Fixtures;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// <see cref="ExecuteSqlTask"/> facts.
    /// </summary>
    public class ExecuteSqlTaskFacts : DefaultTaskContextFixture
    {
        /// <summary>
        /// Default connection factory.
        /// </summary>
        IDbConnectionFactory connectionFactory = new DbConnectionFactory("Data Source=(localdb)\v11.0");

        /// <summary>
        /// Throws when null connection factory passed.
        /// </summary>
        [Fact]
        public static void ThrowsWhenNullConnectionFactoryPassed()
        {
            var exception = 
                Assert.Throws<ArgumentNullException>(() => new ExecuteSqlTask(null, "<not tested>"));
            
            Assert.Equal("connectionFactory", exception.ParamName);
        }

        /// <summary>
        /// Throws when null command text passed.
        /// </summary>
        [Fact]
        public void ThrowsWhenNullCommandTextPassed()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() => new ExecuteSqlTask(this.connectionFactory, null));

            Assert.Equal("commandText", exception.ParamName);
        }

        /// <summary>
        /// Throws when empty command text passed.
        /// </summary>
        [Fact]
        public void ThrowsWhenEmptyCommandTextPassed()
        {
            var exception =
                Assert.Throws<ArgumentException>(() => new ExecuteSqlTask(this.connectionFactory, "  "));

            Assert.Equal("commandText", exception.ParamName);
        }

        /// <summary>
        /// Throws when an invalid connection is used for validation.
        /// </summary>
        [Fact]
        public void ThrowsWhenValidatingInvalidConnection()
        {
            var invalidConnectionFactory = Substitute.For<IDbConnectionFactory>();

            invalidConnectionFactory
                .When(x => x.Validate())
                .Do(x => { throw new InvalidOperationException("Test exception"); });

            var task = new ExecuteSqlTask(invalidConnectionFactory, "select top 1 * from sys.objects");

            Assert.Throws<AggregateException>(() => task.VerifyAsync(this.Context).Wait());
        }

        /// <summary>
        /// Correct query is passed to database.
        /// </summary>
        [Fact]
        public void ExecutePassesCorrectQueryToDatabase()
        {
            var command = Substitute.For<IDbCommand>();

            var connection = Substitute.For<IDbConnection>();
            connection.CreateCommand().Returns(command);

            var factory = Substitute.For<IDbConnectionFactory>();
            factory.CreateConnection().Returns(connection);

            var query = "select top 1 * from sys.objects";

            var task = new ExecuteSqlTask(factory, "select top 1 * from sys.objects");
            
            task.Execute(this.Context);

            command.Received().CommandText = query;
            command.Received().ExecuteNonQuery();
        }
    }
}
