#region Copyright (c) 2013 James Snape
// <copyright file="SimpleContextFixture.cs" company="James Snape">
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
    using System.Threading;
    using Autofac;
    using Autofac.Extras.CommonServiceLocator;
    using Microsoft.Practices.ServiceLocation;
    using Ploeh.AutoFixture;

    /// <summary>
    /// SimpleTaskContextFixture class definition.
    /// </summary>
    /// <remarks>
    /// Depending on which is simpler, this class can either be inherited by test classes
    /// or created and disposed inside test. In the first instance XUnit will automatically
    /// dispose of the instance but in the second you should use a using or finally block
    /// to ensure correct disposal.
    /// </remarks>
    public class SimpleContextFixture : IDisposable
    {
        /// <summary>
        /// Fake context for tests.
        /// </summary>
        private readonly TaskContext context;

        /// <summary>
        /// Cancellation token source.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Test fixture instance.
        /// </summary>
        private readonly Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContextFixture"/> class.
        /// </summary>
        /// <remarks>This constructor is normally called when used as a test base class.</remarks>
        public SimpleContextFixture()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContextFixture"/> class.
        /// </summary>
        /// <remarks>This constructor is normally used when instantiated in-test.</remarks>
        /// <param name="registerTypes">Call-back function allowing the user to register additional classes.</param>
        public SimpleContextFixture(Action<ContainerBuilder> registerTypes)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleExecutionLog>().As<IExecutionLog>();

            if (registerTypes != null)
            {
                registerTypes(builder);
            }

            var container = builder.Build();
            var locator = new AutofacServiceLocator(container);

            ServiceLocator.SetLocatorProvider(() => locator);

            this.fixture = new Fixture();

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
        /// Gets the test data fixture.
        /// </summary>
        public Fixture Fixture
        {
            get { return this.fixture; }
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
