#region Copyright (c) 2013 James Snape
// <copyright file="DefaultTaskContextFixture.cs" company="James Snape">
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

namespace Deeply.Tests.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// DefaultTaskContextFixture class definition.
    /// </summary>
    public class DefaultTaskContextFixture : IDisposable
    {
        /// <summary>
        /// Service locator fixture.
        /// </summary>
        private readonly SetupServiceLocatorFixture serviceLocatorFixture;

        /// <summary>
        /// Fake context for tests.
        /// </summary>
        private readonly TaskContext context;

        /// <summary>
        /// Cancellation token source.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTaskContextFixture"/> class.
        /// </summary>
        public DefaultTaskContextFixture()
        {
            this.serviceLocatorFixture = new SetupServiceLocatorFixture();
            this.serviceLocatorFixture.ConfigureServiceLocator();

            this.cancellationTokenSource = new CancellationTokenSource();
            this.context = new TaskContext(this.cancellationTokenSource);
        }

        /// <summary>
        /// Gets the task context.
        /// </summary>
        public ITaskContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A flag indicating whether this instance is being disposed or finalized.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.cancellationTokenSource.Dispose();
        }
    }
}
