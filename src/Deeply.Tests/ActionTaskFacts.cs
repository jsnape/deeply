#region Copyright (c) 2013 James Snape
// <copyright file="ActionTaskFacts.cs" company="James Snape">
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
    using System.Threading.Tasks;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    /// <summary>
    /// ActionTask facts.
    /// </summary>
    public static class ActionTaskFacts
    {
        /// <summary>
        /// Throws when null action passed.
        /// </summary>
        [Fact]
        public static void ThrowsWhenNullActionPassed()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ActionTask(null));
            Assert.Equal("action", exception.ParamName);
        }

        /// <summary>
        /// Action function called when task executed.
        /// </summary>
        /// <returns>A task representing the completion of this function.</returns>
        [Fact]
        public static async Task ActionFunctionCalledWhenExecuted()
        {
            bool wasCalled = false;
            var task = new ActionTask(() => wasCalled = true);

            var context = Substitute.For<ITaskContext>();

            await task.ExecuteAsync(context);

            wasCalled.Should().BeTrue();
        }
    }
}
