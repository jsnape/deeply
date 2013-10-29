#region Copyright (c) 2013 James Snape
// <copyright file="DateSourceFacts.cs" company="James Snape">
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
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// DateSource facts
    /// </summary>
    public static class DateSourceFacts
    {
        /// <summary>
        /// Return an empty sequence when the same date is passed for both arguments.
        /// </summary>
        [Fact]
        public static void ShouldReturnEmptySequenceWhenSameDateIsPassed()
        {
            var date = new DateTime(2013, 10, 11);

            var sequence = DateSource.GetDateSequence(date, date);

            sequence.Should().BeEmpty();
        }

        /// <summary>
        /// Return the from date only when consecutive days passed.
        /// </summary>
        [Fact]
        public static void ShouldReturnFromDateItemWhenConsecutiveDaysPassed()
        {
            var minDate = new DateTime(2013, 10, 11);
            var maxDate = new DateTime(2013, 10, 12);

            var sequence = DateSource.GetDateSequence(minDate, maxDate);

            sequence.Single().Should().Be(minDate);
        }

        /// <summary>
        /// Return the an item for each day difference between the from and to dates.
        /// </summary>
        [Fact]
        public static void ShouldReturnACountOfDateDiffDays()
        {
            int expectedCount = 10;
            var minDate = new DateTime(2013, 10, 11);
            var maxDate = minDate.AddDays(expectedCount);

            var sequence = DateSource.GetDateSequence(minDate, maxDate);

            sequence.Count().Should().Be(expectedCount);
        }
    }
}
