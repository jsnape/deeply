#region Copyright (c) 2013 James Snape
// <copyright file="EnumerableDataReader.cs" company="James Snape">
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
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// EnumerableDataReader class definition.
    /// </summary>
    /// <typeparam name="T">Enumerated type</typeparam>
    public class EnumerableDataReader<T> : IDataReader
    {
        /// <summary>
        /// Member accessors.
        /// </summary>
        private readonly Func<T, object>[] accessors;

        /// <summary>
        /// Ordinal map since most <c>SqlBulkCopy</c> calls come by ordinal.
        /// </summary>
        private readonly Dictionary<string, int> ordinalLookup;

        /// <summary>
        /// Enumerator instance.
        /// </summary>
        private IEnumerator<T> enumerator;

        /// <summary>
        /// Initializes a new instance of the EnumerableDataReader class.
        /// </summary>
        /// <param name="data">An <c>IEnumerable</c> instance to adapt.</param>
        public EnumerableDataReader(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.enumerator = data.GetEnumerator();

            var propertyAccessors = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .Select((p, i) => new
                {
                    Index = i,
                    Property = p,
                    Accessor = CreatePropertyAccessor(p)
                })
                .ToArray();

            this.accessors = propertyAccessors.Select(p => p.Accessor).ToArray();

            this.ordinalLookup = propertyAccessors.ToDictionary(
               p => p.Property.Name,
               p => p.Index,
               StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>The field count.</value>
        /// <returns>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.</returns>
        public int FieldCount
        {
            get { return this.accessors.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        /// <returns>true if the data reader is closed; otherwise, false.</returns>
        public bool IsClosed
        {
            get { return this.enumerator == null; }
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value>The depth.</value>
        /// <returns>The level of nesting.</returns>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <value>The records affected.</value>
        /// <returns>The number of rows changed, inserted, or deleted; 0 if no rows were affected or the statement failed; and -1 for SELECT statements.</returns>
        public int RecordsAffected
        {
            get { return -1; }
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The value for the named column.</returns>
        public object this[string name]
        {
            get { return this.GetValue(this.GetOrdinal(name)); }
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>The value for the column index.</returns>
        public object this[int i]
        {
            get { return this.GetValue(i); }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Advances the <see cref="T:System.Data.IDataReader" /> to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        /// <exception cref="System.ObjectDisposedException">Thrown when the instance has been disposed.</exception>
        public bool Read()
        {
            if (this.enumerator == null)
            {
                throw new ObjectDisposedException("EnumerableDataReader");
            }

            return this.enumerator.MoveNext();
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when unknown parameter specified.</exception>
        public int GetOrdinal(string name)
        {
            int ordinal;

            if (!this.ordinalLookup.TryGetValue(name, out ordinal))
            {
                throw new InvalidOperationException("Unknown parameter name " + name);
            }

            return ordinal;
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <remarks>This function is order N so don't use it in a loop.</remarks>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when invalid index it passed.</exception>
        public string GetName(int i)
        {
            foreach (var item in this.ordinalLookup.Keys)
            {
                if (this.ordinalLookup[item] == i)
                {
                    return item;
                }
            }

            throw new ArgumentOutOfRangeException("Unknown ordinal " + i.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Object" /> which will contain the field value upon return.</returns>
        /// <exception cref="System.ObjectDisposedException">Thrown when the instance has been disposed.</exception>
        public object GetValue(int i)
        {
            if (this.enumerator == null)
            {
                throw new ObjectDisposedException("ObjectDataReader");
            }

            return this.accessors[i](this.enumerator.Current);
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        public bool IsDBNull(int i)
        {
            return this.GetValue(i) == null;
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.IDataReader" /> Object.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public bool NextResult()
        {
            return false;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public bool GetBoolean(int i)
        {
            return (bool)this.GetValue(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        public byte GetByte(int i)
        {
            return (byte)this.GetValue(i);
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        public char GetChar(int i)
        {
            return (char)this.GetValue(i);
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        public DateTime GetDateTime(int i)
        {
            return (DateTime)this.GetValue(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public decimal GetDecimal(int i)
        {
            return (decimal)this.GetValue(i);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        public double GetDouble(int i)
        {
            return (double)this.GetValue(i);
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.</returns>
        public Type GetFieldType(int i)
        {
            return this.GetValue(i).GetType();
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        public float GetFloat(int i)
        {
            return (float)this.GetValue(i);
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        public Guid GetGuid(int i)
        {
            return (Guid)this.GetValue(i);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public short GetInt16(int i)
        {
            return (short)this.GetValue(i);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public int GetInt32(int i)
        {
            return (int)this.GetValue(i);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public long GetInt64(int i)
        {
            return (long)this.GetValue(i);
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public string GetString(int i)
        {
            return (string)this.GetValue(i);
        }

        #region Not Implemented

        /// <summary>
        /// Returns a <see cref="T:System.Data.DataTable" /> that describes the column meta data of the <see cref="T:System.Data.IDataReader" />.
        /// </summary>
        /// <returns>A <see cref="T:System.Data.DataTable" /> that describes the column meta data.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object" /> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
        /// <exception cref="System.NotImplementedException">Throws always.</exception>
        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.enumerator != null)
                {
                    this.enumerator.Dispose();
                    this.enumerator = null;
                }
            }
        }

        /// <summary>
        /// Creates the property accessor.
        /// </summary>
        /// <param name="info">The property meta data to generate a accessor for.</param>
        /// <returns>A compiled property get function.</returns>
        private static Func<T, object> CreatePropertyAccessor(PropertyInfo info)
        {
            // Define the parameter that will be passed - will be the current object
            var parameter = Expression.Parameter(typeof(T), "input");

            // Define an expression to get the value from the property
            var propertyAccess = Expression.Property(parameter, info.GetGetMethod());

            // Make sure the result of the get method is cast as an object
            var castAsObject = Expression.TypeAs(propertyAccess, typeof(object));

            // Create a lambda expression for the property access and compile it
            var lambda = Expression.Lambda<Func<T, object>>(castAsObject, parameter);

            return lambda.Compile();
        }
    }
}
