Deeply
======

A .NET ETL library.

# Features #

* Uses the [Task Parallel Library](http://msdn.microsoft.com/en-us/library/dd460717.aspx) for asynchronous workflows
* Data flows built using Linq
* More code less clicks

# Why? #

This post explains things pretty well: http://snape.me/2013/09/29/im-fed-up-with-sql-server-integration-services/

# Release Notes #

## Deeply 0.2.0-alpha ##

Added support for the following task types:

* Execute SQL
* Execute Process
* Simple Dataflow

This last one is designed to cover a common case where you have a stream of data that is mapped into a target entity type before begin pushed into a database via bulk copy. The mapping function can be as simple or complex as required. 

## Deeply 0.1.0-alpha ##

So far only the sequential and parallel task workflow components are complete along with the ability to execute SQL statements.

# Future Plans #

I only plan on adding features that are commonly useful since this is a library and you can add anything missing.

The basic set of tasks available are able to perform useful tasks so I'm going to set about building an importer for [AdventureWorks](http://msftdbprodsamples.codeplex.com/releases/view/93587) and see how things go before deciding on more features.

# Getting Started #

* Add Deeply from http://nuget.org
* Look at the unit tests here on Github for some examples

*Don't use this in production code just yet.*