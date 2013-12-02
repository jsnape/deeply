#region Copyright (c) 2013 James Snape
// <copyright file="ActionTask.cs" company="James Snape">
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
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// <c>ActionTask</c> class definition.
    /// </summary>
    public class ActionTask : TaskBase
    {
        /// <summary>
        /// Action to execute when the task runs.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="action">Action to call when the task executes.</param>
        public ActionTask(Action action)
            : this(TaskBase.NextTaskName(), action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="action">Action to call when the task executes.</param>
        public ActionTask(string name, Action action)
            : base(name)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override Task ExecuteInternalAsync(ITaskContext context)
        {
            this.action();
            return Task.FromResult<int>(0);
        }
    }
}
