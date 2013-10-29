#region Copyright (c) 2013 James Snape
// <copyright file="DateSource.cs" company="James Snape">
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

namespace Deeply
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Date Source class definition.
    /// </summary>
    public static class DateSource
    {
        /// <summary>
        /// Generates a sequence of dates between two dates.
        /// </summary>
        /// <remarks>The upper bound is exclusive.</remarks>
        /// <param name="minDate">Start date of sequence.</param>
        /// <param name="maxDate">End date of sequence.</param>
        /// <returns>A sequence of <c>DateTime</c> values.</returns>
        public static IEnumerable<DateTime> GetDateSequence(DateTime minDate, DateTime maxDate)
        {
            while (minDate < maxDate)
            {
                yield return minDate;
                minDate = minDate.AddDays(1);
            }
        }
    }
}
