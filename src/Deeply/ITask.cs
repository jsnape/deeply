#region Copyright (c) 2013 James Snape
// <copyright file="ITask.cs" company="James Snape">
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
    using System.Threading.Tasks;

    /// <summary>
    /// ITask interface definition.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Gets the task name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this task is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Executes the task synchronously.
        /// </summary>
        /// <remarks>The default implementation just calls <c>ExecuteAsync</c> and waits for the call to complete.</remarks>
        /// <param name="context">Execution context.</param>
        void Execute(ITaskContext context);

        /// <summary>
        /// Executes the task asynchronously.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        Task ExecuteAsync(ITaskContext context);

        /// <summary>
        /// Verifies the task synchronously.
        /// </summary>
        /// <remarks>The default implementation just calls <c>VerifyAsync</c> and waits for the call to complete.</remarks>
        /// <param name="context">Verification context.</param>
        void Verify(ITaskContext context);

        /// <summary>
        /// Verifies the task asynchronously.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        Task VerifyAsync(ITaskContext context);
    }
}
