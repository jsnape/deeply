#region Copyright (c) 2013 James Snape
// <copyright file="ExecuteSqlTask.cs" company="James Snape">
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
    /// <c>ExecuteSqlTask</c> class definition.
    /// </summary>
    public class ExecuteSqlTask : TaskBase
    {
        /// <summary>
        /// Connection factory.
        /// </summary>
        private readonly IDbConnectionFactory connectionFactory;

        /// <summary>
        /// Command to execute.
        /// </summary>
        private readonly string commandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteSqlTask"/> class.
        /// </summary>
        /// <param name="connectionFactory">Connection factory.</param>
        /// <param name="commandText">Command to execute.</param>
        public ExecuteSqlTask(IDbConnectionFactory connectionFactory, string commandText)
            : this(TaskBase.NextTaskName(), connectionFactory, commandText)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteSqlTask"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="connectionFactory">Connection factory.</param>
        /// <param name="commandText">Command to execute.</param>
        public ExecuteSqlTask(string name, IDbConnectionFactory connectionFactory, string commandText)
            : base(name)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException("connectionFactory");
            }

            if (commandText == null)
            {
                throw new ArgumentNullException("commandText");
            }

            if (string.IsNullOrWhiteSpace(commandText))
            {
                throw new ArgumentException("Command text argument cannot be empty", "commandText");
            }

            this.connectionFactory = connectionFactory;
            this.commandText = commandText;
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override async Task ExecuteInternalAsync(ITaskContext context)
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {
                connection.Open();
                
                var command = connection.CreateCommand();
                command.CommandText = this.commandText;

                var asyncCommand = command as DbCommand;

                if (asyncCommand == null)
                {
                    // This command doesn't support asynchronous execution
                    // so we will tie up this thread executing normally.
                    // In this instance it is OK because there is no 
                    // message pump that we need to return control to.
                    command.ExecuteNonQuery();
                }
                else
                {
                    await asyncCommand.ExecuteNonQueryAsync(context.CancellationToken);
                }
            }
        }

        /// <summary>
        /// Implementation function for the verification.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        protected override async Task VerifyInternalAsync(ITaskContext context)
        {
            await base.VerifyInternalAsync(context);

            this.connectionFactory.Validate();
        }
    }
}
