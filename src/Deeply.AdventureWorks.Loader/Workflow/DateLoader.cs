#region Copyright (c) 2013 James Snape
// <copyright file="DateLoader.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader.Workflow
{
    using System;
    using System.Collections.Generic;
    using Deeply.AdventureWorks.Loader.Domain;

    /// <summary>
    /// Date Loader class definition.
    /// </summary>
    public class DateLoader
    {
        /// <summary>
        /// Target factory.
        /// </summary>
        private readonly IDbConnectionFactory targetFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLoader"/> class.
        /// </summary>
        /// <param name="targetFactory">Target connection factory.</param>
        public DateLoader(IDbConnectionFactory targetFactory)
        {
            if (targetFactory == null)
            {
                throw new ArgumentNullException("targetFactory");
            }

            this.targetFactory = targetFactory;
        }

        /// <summary>
        /// Builds a task used to load the date dimension.
        /// </summary>
        /// <returns>A task used to load the date dimension.</returns>
        public ITask Build()
        {
            var dateTarget = new SqlBulkRepository<DateValue>(
                "dbo.DimDate",
                this.targetFactory,
                new Dictionary<string, string>()
                {
                    { "Key", "DateKey" },
                    { "Value", "FullDateAlternateKey" },
                    { "DayNumberOfWeek", "DayNumberOfWeek" },
                    { "EnglishDayNameOfWeek", "EnglishDayNameOfWeek" },
                    { "SpanishDayNameOfWeek", "SpanishDayNameOfWeek" },
                    { "FrenchDayNameOfWeek", "FrenchDayNameOfWeek" },
                    { "DayNumberOfMonth", "DayNumberOfMonth" },
                    { "DayNumberOfYear", "DayNumberOfYear" },
                    { "WeekNumberOfYear", "WeekNumberOfYear" },
                    { "EnglishMonthName", "EnglishMonthName" },
                    { "SpanishMonthName", "SpanishMonthName" },
                    { "FrenchMonthName", "FrenchMonthName" },
                    { "MonthNumberOfYear", "MonthNumberOfYear" },
                    { "CalendarQuarter", "CalendarQuarter" },
                    { "CalendarYear", "CalendarYear" },
                    { "CalendarSemester", "CalendarSemester" },
                    { "FiscalQuarter", "FiscalQuarter" },
                    { "FiscalYear", "FiscalYear" },
                    { "FiscalSemester", "FiscalSemester" }
                });

            var dateSource = DateSource.GetDateSequence(new DateTime(2005, 01, 01), new DateTime(2010, 12, 31));

            return new SimpleDataflowTask<DateTime, DateValue>(
                "Load date dimension",
                dateSource,
                s => new DateValue(s),
                dateTarget);
        }
    }
}
