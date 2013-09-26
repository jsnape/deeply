#region Copyright (c) 2013 James Snape
// <copyright file="FakeTask.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// FakeTask class definition.
    /// </summary>
    public class FakeTask : TaskBase
    {
        /// <summary>
        /// Initializes a new instance of the FakeTask class.
        /// </summary>
        public FakeTask()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FakeTask class.
        /// </summary>
        /// <param name="name">Task name.</param>
        public FakeTask(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not the <c>ExecuteInternal</c> method was called.
        /// </summary>
        public bool ExecuteWasCalled { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the <c>ExecuteInternal</c> method was called.
        /// </summary>
        public bool VerifyWasCalled { get; private set; }

        /// <summary>
        /// Gets or sets the function called to handle the execute.
        /// </summary>
        public Func<ITaskContext, Task> ExecuteFunction { get; set; }

        /// <summary>
        /// Gets or sets the function called to handle the verify.
        /// </summary>
        public Func<ITaskContext, Task> VerifyFunction { get; set; }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override async Task ExecuteInternalAsync(ITaskContext context)
        {
            this.ExecuteWasCalled = true;

            if (this.ExecuteFunction != null)
            {
                await this.ExecuteFunction(context);
            }
        }

        /// <summary>
        /// Implementation function for the verification.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        protected override async Task VerifyInternalAsync(ITaskContext context)
        {
            this.VerifyWasCalled = true;

            if (this.VerifyFunction != null)
            {
                await this.VerifyFunction(context);    
            }
        }
    }
}
