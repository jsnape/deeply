#region Copyright (c) 2013 James Snape
// <copyright file="Customer.cs" company="James Snape">
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

    /// <summary>
    /// Customer entity definition.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the geography key.
        /// </summary>
        public int GeographyKey { get; set; }

        /// <summary>
        /// Gets or sets the alternate key.
        /// </summary>
        public string AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the name is styled.
        /// </summary>
        public bool? NameStyle { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gets or sets the marital status.
        /// </summary>
        public char? MaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public char? Gender { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the yearly income.
        /// </summary>
        public decimal? YearlyIncome { get; set; }

        /// <summary>
        /// Gets or sets the total number of children.
        /// </summary>
        public byte? TotalChildren { get; set; }

        /// <summary>
        /// Gets or sets the number of children at home.
        /// </summary>
        public byte? NumberChildrenAtHome { get; set; }

        /// <summary>
        /// Gets or sets the education in English.
        /// </summary>
        public string EnglishEducation { get; set; }

        /// <summary>
        /// Gets or sets the education in Spanish.
        /// </summary>
        public string SpanishEducation { get; set; }

        /// <summary>
        /// Gets or sets the education in French.
        /// </summary>
        public string FrenchEducation { get; set; }

        /// <summary>
        /// Gets or sets the occupation in English.
        /// </summary>
        public string EnglishOccupation { get; set; }

        /// <summary>
        /// Gets or sets the occupation in Spanish.
        /// </summary>
        public string SpanishOccupation { get; set; }

        /// <summary>
        /// Gets or sets the occupation in French.
        /// </summary>
        public string FrenchOccupation { get; set; }

        /// <summary>
        /// Gets or sets the house owner state.
        /// </summary>
        public char? IsHouseOwner { get; set; }

        /// <summary>
        /// Gets or sets the number of cars owned.
        /// </summary>
        public int? NumberCarsOwned { get; set; }

        /// <summary>
        /// Gets or sets the address line 1..
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the first purchase date.
        /// </summary>
        public DateTime FirstPurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the commute distance.
        /// </summary>
        public string CommuteDistance { get; set; }
    }
}
