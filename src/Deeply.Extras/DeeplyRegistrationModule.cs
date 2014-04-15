#region Copyright (c) 2013 James Snape
// <copyright file="DeeplyRegistrationModule.cs" company="James Snape">
//  Copyright 2014 James Snape
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

namespace Deeply.Extras
{
    using Autofac;
    using Deeply;

    /// <summary>
    /// Registers Deeply types.
    /// </summary>
    public class DeeplyRegistrationModule : Module
    {
        /// <summary>
        /// Loads the supplied builder with module specific types.
        /// </summary>
        /// <param name="builder">Container builder instance.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleExecutionLog>().As<IExecutionLog>();
        }
    }
}
