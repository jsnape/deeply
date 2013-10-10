#region Copyright (c) 2013 James Snape
// <copyright file="EnumerableDataReaderFacts.cs" company="James Snape">
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
    using System.Globalization;
    using System.Linq;
    using Deeply.Tests.Fakes;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Xunit;

    /// <summary>
    /// EnumerableDataReader Facts
    /// </summary>
    public sealed class EnumerableDataReaderFacts : IDisposable
    {
        /// <summary>
        /// Test data fixture.
        /// </summary>
        private readonly Fixture fixture;

        /// <summary>
        /// Test data.
        /// </summary>
        private readonly IEnumerable<FakeEntity> data;

        /// <summary>
        /// Class under test.
        /// </summary>
        private readonly EnumerableDataReader<FakeEntity> reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableDataReaderFacts"/> class.
        /// </summary>
        public EnumerableDataReaderFacts()
        {
            this.fixture = new Fixture();
            this.data = this.fixture.CreateMany<FakeEntity>();

            this.reader = new EnumerableDataReader<FakeEntity>(this.data);
        }

        /// <summary>
        /// Throws when null passed to constructor.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Deeply.EnumerableDataReader`1<Deeply.Tests.Fakes.FakeEntity>", Justification = "Call throws so no object returned.")]
        [Fact]
        public static void ThrowsWhenNullEnumerablePassed()
        {
            Action action = () =>
            {
                using (new EnumerableDataReader<FakeEntity>(null))
                {
                }
            };

            action.ShouldThrow<ArgumentNullException>()
                .Where(e => e.ParamName == "data");
        }

        /// <summary>
        /// Field count should be correct.
        /// </summary>
        [Fact]
        public void FieldCountShouldBeCorrect()
        {
            var properties = typeof(FakeEntity).GetProperties();
            var expectedCount = properties.Length;

            this.reader.FieldCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Property values should be correct.
        /// </summary>
        /// <remarks>
        /// This test is quite long and has multiple asserts but it is the 
        /// simplest way to test all the properties with minimum repeated code.
        /// </remarks>
        [Fact]
        public void PropertyValuesShouldBeCorrect()
        {
            var firstItem = this.data.First();
            
            this.reader.Read();

            foreach (var property in typeof(FakeEntity).GetProperties())
            {
                var value = property.GetValue(firstItem);

                int ordinal = this.reader.GetOrdinal(property.Name);

                // Test the GetSchemaType
                Assert.Equal(property.PropertyType, this.reader.GetFieldType(ordinal));

                // Test the name -> ordinal -> name equivalence.
                Assert.Equal(property.Name, this.reader.GetName(ordinal));

                // Test the index accessors
                Assert.Equal(value, this.reader[property.Name]);
                Assert.Equal(value, this.reader[ordinal]);

                switch (property.PropertyType.Name)
                {
                    case "Guid":
                        Assert.Equal((Guid)value, this.reader.GetGuid(ordinal));
                        break;
                   case "Boolean":
                        Assert.Equal((bool)value, this.reader.GetBoolean(ordinal));
                        break;
                   case "Byte":
                        Assert.Equal((byte)value, this.reader.GetByte(ordinal));
                        break;
                   case "Char":
                        Assert.Equal((char)value, this.reader.GetChar(ordinal));
                        break;
                   case "DateTime":
                        Assert.Equal((DateTime)value, this.reader.GetDateTime(ordinal));
                        break;
                   case "Decimal":
                        Assert.Equal((decimal)value, this.reader.GetDecimal(ordinal));
                        break;
                   case "Double":
                        Assert.Equal((double)value, this.reader.GetDouble(ordinal));
                        break;
                   case "Single":
                        Assert.Equal((float)value, this.reader.GetFloat(ordinal));
                        break;
                   case "String":
                        Assert.Equal((string)value, this.reader.GetString(ordinal));
                        break;
                    case "Int16":
                        Assert.Equal((short)value, this.reader.GetInt16(ordinal));
                        break;
                    case "Int32":
                        Assert.Equal((int)value, this.reader.GetInt32(ordinal));
                        break;
                    case "Int64":
                        Assert.Equal((long)value, this.reader.GetInt64(ordinal));
                        break;
                }
            }
        }

        /// <summary>
        /// Once disposed should throw.
        /// </summary>
        [Fact]
        public void ThrowsWhenReadAfterDisposed()
        {
            var localReader = new EnumerableDataReader<FakeEntity>(this.data);

            localReader.IsClosed.Should().BeFalse();

            localReader.Close();

            localReader.IsClosed.Should().BeTrue();

            Action act = () => localReader.Read();

            act.ShouldThrow<ObjectDisposedException>()
                .Where(ex => ex.ObjectName == "EnumerableDataReader");
        }

        /// <summary>
        /// Throws a value is requested after disposal.
        /// </summary>
        [Fact]
        public void ThrowsWhenValueAccessedAfterDisposal()
        {
            var localReader = new EnumerableDataReader<FakeEntity>(this.data);

            localReader.Close();

            Action act = () => localReader.GetValue(0);

            act.ShouldThrow<ObjectDisposedException>();
        }

        /// <summary>
        /// Throws when invalid name used as a lookup.
        /// </summary>
        [Fact]
        public void ThrowsWhenInvalidNameUsed()
        {
            var invalidName = "DOES NOT EXIST";
            Action act = () => this.reader.GetOrdinal(invalidName);

            act.ShouldThrow<InvalidOperationException>()
                .Where(ex => ex.Message.Contains(invalidName));
        }

        /// <summary>
        /// Throws when invalid index used as a lookup.
        /// </summary>
        [Fact]
        public void ThrowsWhenInvalidIndexUsed()
        {
            var invalidIndex = -1;
            Action act = () => this.reader.GetName(invalidIndex);

            act.ShouldThrow<ArgumentOutOfRangeException>()
                .Where(ex => ex.Message.Contains(invalidIndex.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Called to release resources.
        /// </summary>
        public void Dispose()
        {
            this.reader.Close();
        }
    }
}
