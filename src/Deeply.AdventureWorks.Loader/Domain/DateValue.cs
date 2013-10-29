#region Copyright (c) 2013 James Snape
// <copyright file="DateValue.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Domain class for the date dimension.
    /// </summary>
    public class DateValue
    {
        /// <summary>
        /// English culture.
        /// </summary>
        private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");

        /// <summary>
        /// French culture.
        /// </summary>
        private static readonly CultureInfo FrenchCulture = CultureInfo.GetCultureInfo("fr-FR");

        /// <summary>
        /// Spanish culture.
        /// </summary>
        private static readonly CultureInfo SpanishCulture = CultureInfo.GetCultureInfo("es-ES");

        /// <summary>
        /// Standard calendar.
        /// </summary>
        private static readonly GregorianCalendar Calendar = new GregorianCalendar();

        /// <summary>
        /// The internal date value.
        /// </summary>
        private readonly DateTime value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateValue"/> class.
        /// </summary>
        /// <param name="value">Date value.</param>
        public DateValue(DateTime value)
        {
            this.value = value.Date;
        }

        /// <summary>
        /// Gets the date key.
        /// </summary>
        public int Key
        {
            get { return (this.value.Year * 10000) + (this.value.Month * 100) + this.value.Day; }
        }

        /// <summary>
        /// Gets the date value.
        /// </summary>
        public DateTime Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the day number of week.
        /// </summary>
        public byte DayNumberOfWeek
        {
            get { return (byte)this.value.DayOfWeek; }
        }

        /// <summary>
        /// Gets the English day name of week.
        /// </summary>
        public string EnglishDayNameOfWeek
        {
            get { return this.value.ToString("dddd", EnglishCulture); }
        }

        /// <summary>
        /// Gets the Spanish day name of week.
        /// </summary>
        public string SpanishDayNameOfWeek
        {
            get { return this.value.ToString("dddd", SpanishCulture); }
        }

        /// <summary>
        /// Gets the French day name of week.
        /// </summary>
        public string FrenchDayNameOfWeek
        {
            get { return this.value.ToString("dddd", FrenchCulture); }
        }

        /// <summary>
        /// Gets the day number of month.
        /// </summary>
        public byte DayNumberOfMonth
        {
            get { return (byte)this.value.Day; }
        }

        /// <summary>
        /// Gets the day number of year.
        /// </summary>
        [CLSCompliant(false)]
        public ushort DayNumberOfYear
        {
            get { return (ushort)this.value.DayOfYear; }
        }

        /// <summary>
        /// Gets the week number of year.
        /// </summary>
        public byte WeekNumberOfYear
        {
            get { return (byte)Calendar.GetWeekOfYear(this.value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday); }
        }

        /// <summary>
        /// Gets the English month name.
        /// </summary>
        public string EnglishMonthName
        {
            get { return this.value.ToString("MMMM", EnglishCulture); }
        }

        /// <summary>
        /// Gets the Spanish month name.
        /// </summary>
        public string SpanishMonthName
        {
            get { return this.value.ToString("MMMM", SpanishCulture); }
        }

        /// <summary>
        /// Gets the French month name.
        /// </summary>
        public string FrenchMonthName
        {
            get { return this.value.ToString("MMMM", FrenchCulture); }
        }

        /// <summary>
        /// Gets the month number of year.
        /// </summary>
        public byte MonthNumberOfYear
        {
            get { return (byte)this.value.Month; }
        }

        /// <summary>
        /// Gets the calendar quarter of year.
        /// </summary>
        public byte CalendarQuarter
        {
            get 
            {
                if (this.value.Month < 4)
                {
                    return 1;
                }

                if (this.value.Month < 7)
                {
                    return 2;
                }

                if (this.value.Month < 10)
                {
                    return 3;
                }

                return 4; 
            }
        }

        /// <summary>
        /// Gets the calendar year.
        /// </summary>
        public int CalendarYear
        {
            get { return this.value.Year; }
        }

        /// <summary>
        /// Gets the calendar semester.
        /// </summary>
        public int CalendarSemester
        {
            get { return this.value.Month < 7 ? 1 : 2; }
        }

        /// <summary>
        /// Gets the fiscal quarter.
        /// </summary>
        public byte FiscalQuarter
        {
            get 
            {
                int fiscalQuarter = this.CalendarQuarter + 2;
                
                if (fiscalQuarter > 4)
                {
                    fiscalQuarter -= 4;
                }

                return (byte)fiscalQuarter;
            }
        }

        /// <summary>
        /// Gets the fiscal year.
        /// </summary>
        public int FiscalYear
        {
            get { return this.value.Month < 7 ? this.value.Year : this.value.Year + 1; }
        }

        /// <summary>
        /// Gets the fiscal semester.
        /// </summary>
        public byte FiscalSemester
        {
            get 
            {
                int fiscalSemester = this.CalendarSemester + 1;

                if (fiscalSemester > 2)
                {
                    fiscalSemester -= 2;
                }

                return (byte)fiscalSemester;
            }
        }
    }
}
