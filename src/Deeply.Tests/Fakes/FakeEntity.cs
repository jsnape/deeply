#region Copyright (c) 2013 James Snape
// <copyright file="FakeEntity.cs" company="James Snape">
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

namespace Deeply.Tests.Fakes
{
    using System;
using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Fake entity implementation.
    /// </summary>
    public class FakeEntity
    {
        /// <summary>
        /// Gets or sets the fake id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the fake name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the fake creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the boolean Amount is set.
        /// </summary>
        public bool BooleanAmount { get; set; }

        /// <summary>
        /// Gets or sets the byte Amount.
        /// </summary>
        public byte ByteAmount { get; set; }

        /// <summary>
        /// Gets or sets the char Amount.
        /// </summary>
        public char CharAmount { get; set; }

        /// <summary>
        /// Gets or sets the decimal Amount.
        /// </summary>
        public decimal DecimalAmount { get; set; }

        /// <summary>
        /// Gets or sets the float Amount.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "float", Justification = "Test specific code.")]
        public float FloatAmount { get; set; }

        /// <summary>
        /// Gets or sets the float Amount.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "float", Justification = "Test specific code.")]
        public double DoubleAmount { get; set; }

        /// <summary>
        /// Gets or sets the <c>Guid</c> Amount.
        /// </summary>
        public Guid GuidAmount { get; set; }

        /// <summary>
        /// Gets or sets the short Amount.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "short", Justification = "Test specific code.")]
        public short ShortAmount { get; set; }

        /// <summary>
        /// Gets or sets the long Amount.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = "Test specific code.")]
        public long LongAmount { get; set; }
    }
}
