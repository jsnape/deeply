Deeply
======

A .NET ETL library.

# Features #

* Uses the [Task Parallel Library](http://msdn.microsoft.com/en-us/library/dd460717.aspx) for asynchronous workflows
* Data flows built using Linq (soon)
* More code less clicks

# Why? #

This post explains things pretty well: http://snape.me/2013/09/29/im-fed-up-with-sql-server-integration-services/

# Current Status #

So far only the sequential and parallel task workflow components are complete along with the ability to execute SQL statements.

# Future Plans #

I only plan on adding features that are commonly useful since this is a library and you can add anything missing. However its a high priority to add linq data flows since this is fundamentally what SSIS is for.

# Getting Started #

* Add Deeply from http://nuget.org
* Look at the unit tests here on Github for some examples

*Don't use this in production code just yet.*