#region Copyright (c) 2013 James Snape
// <copyright file="KeyMapper.cs" company="James Snape">
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
    using System.Threading;

    /// <summary>
    /// KeyMapper class definition.
    /// </summary>
    /// <typeparam name="T">Surrogate key type.</typeparam>
    public sealed class KeyMapper<T> : IDisposable, IKeyMapper<T> where T : struct
    {
        /// <summary>
        /// Lookup map.
        /// </summary>
        private readonly Dictionary<string, T> map;

        /// <summary>
        /// Lookup function.
        /// </summary>
        private readonly Func<string, T> lookup;

        /// <summary>
        /// Read/Write lock.
        /// </summary>
        private readonly ReaderWriterLockSlim updateLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes a new instance of the KeyMapper class.
        /// </summary>
        /// <param name="initialCache">Initial cache population data.</param>
        /// <param name="lookup">Individual item lookup function.</param>
        public KeyMapper(IDictionary<string, T> initialCache, Func<string, T> lookup)
        {
            this.lookup = lookup;
            this.map = new Dictionary<string, T>(initialCache);
        }

        /// <summary>
        /// Disposes any managed resources.
        /// </summary>
        public void Dispose()
        {
            this.updateLock.Dispose();
        }

        /// <summary>
        /// Maps the business key to a surrogate key.
        /// </summary>
        /// <param name="businessKey">Business key to map.</param>
        /// <returns>Related surrogate key.</returns>
        public T Map(string businessKey)
        {
            this.updateLock.EnterUpgradeableReadLock();

            try
            {
                T surrogateKey;

                if (this.map.TryGetValue(businessKey, out surrogateKey))
                {
                    return surrogateKey;
                }

                return this.Lookup(businessKey);
            }
            finally
            {
                this.updateLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Finds the surrogate key using the supplied function.
        /// </summary>
        /// <param name="businessKey">Business key to map.</param>
        /// <returns>Related surrogate key.</returns>
        private T Lookup(string businessKey)
        {
            this.updateLock.EnterWriteLock();

            try
            {
                T surrogateKey = this.lookup(businessKey);

                this.map.Add(businessKey, surrogateKey);

                return surrogateKey;
            }
            finally
            {
                this.updateLock.ExitWriteLock();
            }
        }
    }
}
