#region Copyright (c) 2013 James Snape
// <copyright file="DependencyResolverExtensions.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a type-safe implementation of <c>GetService</c> and <c>GetServices</c>.
    /// </summary>
    /// <remarks>This is the same as <see cref="System.Web.Mvc.DependencyResolverExtensions"/>.</remarks>
    public static class DependencyResolverExtensions
    {
        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <typeparam name="TService">The type of the requested service or object.</typeparam>
        /// <param name="resolver">The dependency resolver instance that this method extends.</param>
        /// <returns>The requested service or object</returns>
        public static TService GetService<TService>(this IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            return (TService)resolver.GetService(typeof(TService));
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <typeparam name="TService">The type of the requested services.</typeparam>
        /// <param name="resolver">The dependency resolver instance that this method extends.</param>
        /// <returns>The requested services.</returns>
        public static IEnumerable<TService> GetServices<TService>(this IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            return resolver.GetServices(typeof(TService)).Cast<TService>();
        }
    }
}
