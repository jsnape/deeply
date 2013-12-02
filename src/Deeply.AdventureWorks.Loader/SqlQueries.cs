#region Copyright (c) 2013 James Snape
// <copyright file="SqlQueries.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// All the SQL constants in one place.
    /// </summary>
    public static class SqlQueries
    {
        /// <summary>
        /// Truncate facts SQL.
        /// </summary>
        public const string TruncateFactsSql = @"
truncate table dbo.FactAdditionalInternationalProductDescription;
truncate table dbo.FactCallCenter;
truncate table dbo.FactCurrencyRate;
truncate table dbo.FactFinance;
truncate table dbo.FactProductInventory;
truncate table dbo.FactResellerSales;
truncate table dbo.FactSalesQuota;
truncate table dbo.FactSurveyResponse;

alter table dbo.FactInternetSalesReason
	drop constraint FK_FactInternetSalesReason_FactInternetSales;

truncate table dbo.FactInternetSalesReason;
truncate table dbo.FactInternetSales;

alter table dbo.FactInternetSalesReason
	with check add constraint FK_FactInternetSalesReason_FactInternetSales
	foreign key(SalesOrderNumber, SalesOrderLineNumber)
	references dbo.FactInternetSales (SalesOrderNumber, SalesOrderLineNumber);

alter table dbo.FactInternetSalesReason 
	check constraint FK_FactInternetSalesReason_FactInternetSales;
";

        /// <summary>
        /// Query to delete all the dimension members.
        /// </summary>
        public const string DeleteDimensionData = @"
delete from dbo.DimAccount;
delete from dbo.DimCurrency;
delete from dbo.DimCustomer;
delete from dbo.DimDate;
delete from dbo.DimDepartmentGroup;
delete from dbo.DimEmployee;
delete from dbo.DimGeography;
delete from dbo.DimOrganization;
delete from dbo.DimProduct;
delete from dbo.DimProductSubcategory;
delete from dbo.DimProductCategory;
delete from dbo.DimPromotion;
delete from dbo.DimReseller;
delete from dbo.DimSalesReason;
delete from dbo.DimSalesTerritory;
delete from dbo.DimScenario;
";

        /// <summary>
        /// Product category surrogate key lookup query.
        /// </summary>
        public const string ProductCategoryKeyMap = @"
select 
	ProductCategoryKey, 
	cast(ProductCategoryAlternateKey as nvarchar(20)) as BusinessKey
from dbo.DimProductCategory with (nolock)
";

        /// <summary>
        /// Product subcategory surrogate key lookup query.
        /// </summary>
        public const string ProductSubcategoryKeyMap = @"
select 
	ProductSubcategoryKey, 
	cast(ProductSubcategoryAlternateKey as nvarchar(20)) as BusinessKey
from dbo.DimProductSubcategory with (nolock)
";
    }
}
