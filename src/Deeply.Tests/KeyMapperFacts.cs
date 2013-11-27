#region Copyright (c) 2013 James Snape
// <copyright file="KeyMapperFacts.cs" company="James Snape">
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
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// KeyMapper Facts
    /// </summary>
    public class KeyMapperFacts
    {
        /// <summary>
        /// Helper object.
        /// </summary>
        private readonly Dictionary<string, int> emptyDictionary = new Dictionary<string, int>();

        /// <summary>
        /// Helper object.
        /// </summary>
        private readonly Dictionary<string, int> primedDictionary = new Dictionary<string, int>()
        {
            { "1234", 1234 },
            { "1235", 1235 },
            { "1236", 1236 },
        };

        /// <summary>
        /// Empty cache should lookup first value.
        /// </summary>
        [Fact]
        public void EmptyCacheShouldLookupFirstValue()
        {
            bool lookupCalled = false;

            using (var mapper =
                new KeyMapper<int>(this.emptyDictionary, x => { lookupCalled = true; return 1234; }))
            {
                var key = mapper.Map("1234");

                Assert.Equal(1234, key);
                Assert.Equal(true, lookupCalled);
            }
        }

        /// <summary>
        /// Primed cache should not attempt lookup.
        /// </summary>
        [Fact]
        public void PrimedCacheShouldNotAttemptLookup()
        {
            int lookupCount = 0;

            using (var mapper =
                new KeyMapper<int>(this.primedDictionary, x => { lookupCount++; return 0; }))
            {
                var key = mapper.Map("1234");

                Assert.Equal(1234, key);
                Assert.Equal(0, lookupCount);
            }
        }
    }
}
