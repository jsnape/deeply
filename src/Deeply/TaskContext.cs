#region Copyright (c) 2013 James Snape
// <copyright file="TaskContext.cs" company="James Snape">
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
    using System.Threading;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// TaskContext class definition.
    /// </summary>
    public class TaskContext : ITaskContext
    {
        /// <summary>
        /// Cancellation token source.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Execution log.
        /// </summary>
        private readonly IExecutionLog log;

        /// <summary>
        /// Initializes a new instance of the TaskContext class.
        /// </summary>
        /// <param name="cancellationTokenSource">A cancellation source.</param>
        public TaskContext(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                throw new ArgumentNullException("cancellationTokenSource");
            }

            this.cancellationTokenSource = cancellationTokenSource;

            this.log = ServiceLocator.Current.GetInstance<IExecutionLog>();
        }

        /// <summary>
        /// Gets the execution log.
        /// </summary>
        public IExecutionLog Log 
        { 
            get { return this.log; } 
        }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        public CancellationToken CancellationToken 
        {
            get { return this.cancellationTokenSource.Token; }
        }

        /// <summary>
        /// Communicates a request for cancellation.
        /// </summary>
        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
        }
    }
}
