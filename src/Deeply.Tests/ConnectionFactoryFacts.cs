#region Copyright (c) 2013 James Snape
// <copyright file="ConnectionFactoryFacts.cs" company="James Snape">
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
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Deeply.Tests.Fakes;
    using Deeply.Tests.Fixtures;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// ConnectionFactoryFacts Task tests.
    /// </summary>
    public static class ConnectionFactoryFacts
    {
        /// <summary>
        /// Default connection assumes you have <c>localdb</c> installed.
        /// </summary>
        private const string LocalDbConnection = "Data Source=(localdb)\v11.0;Integrated Security=True;";

        /// <summary>
        /// Throws when created with a null connection string.
        /// </summary>
        [Fact]
        public static void ThrowsWhenCreatedWithNullConnectionString()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new DbConnectionFactory(null));
            Assert.Equal("connectionString", exception.ParamName);
        }

        /// <summary>
        /// Throws when created with an empty connection string.
        /// </summary>
        [Fact]
        public static void ThrowsWhenCreatedWithEmptyConnectionString()
        {
            var exception = Assert.Throws<ArgumentException>(() => new DbConnectionFactory(string.Empty));
            Assert.Equal("connectionString", exception.ParamName);
        }

        /// <summary>
        /// Throws when created with an empty connection string.
        /// </summary>
        [Fact]
        public static void ThrowsWhenCreatedWithNullCreateFunction()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new DbConnectionFactory("NOT USED", null));
            Assert.Equal("createFunction", exception.ParamName);
        }

        /// <summary>
        /// Default factory creates SQL connections.
        /// </summary>
        [Fact]
        public static void DefaultFactoryCreatesSqlConnections()
        {
            var factory = new DbConnectionFactory(LocalDbConnection);
            var connection = factory.CreateConnection();
            Assert.IsType<SqlConnection>(connection);
        }

        /// <summary>
        /// Template factory creates correct type of connection.
        /// </summary>
        [Fact]
        public static void TemplateFactoryCreatesCorrectTypeOfConnection()
        {
            var factory = new DbConnectionFactory<OleDbConnection>("Provider=SQLOLEDB;" + LocalDbConnection);
            var connection = factory.CreateConnection();
            Assert.IsType<OleDbConnection>(connection);
        }

        /// <summary>
        /// When validating any <c>DbExceptions</c> thrown should be cached.
        /// </summary>
        [Fact]
        public static void ValidateCachesException()
        {
            var exception = Substitute.For<DbException>();

            var invalidConnection = Substitute.For<IDbConnection>();

            invalidConnection
                .When(x => x.Open())
                .Do(x =>
                {
                    throw exception;
                });

            var factory = new DbConnectionFactory(LocalDbConnection, c => invalidConnection);

            try
            {
                factory.Validate();
            }
            catch (DbException)
            {
            }

            try
            {
                // This should cache the result and not call open.
                factory.Validate();
            }
            catch (DbException)
            {
            }

            invalidConnection.Received(1).Open();
        }

        /// <summary>
        /// When validating always reopen connections.
        /// </summary>
        [Fact]
        public static void ValidateOpensConnectionEachTime()
        {
            var validConnection = Substitute.For<IDbConnection>();

            var factory = new DbConnectionFactory(LocalDbConnection, c => validConnection);

            const int CallCount = 2;

            for (int i = 0; i < CallCount; ++i)
            {
                Assert.DoesNotThrow(() => factory.Validate());
            }

            validConnection.Received(CallCount).Open();
        }
    }
}
