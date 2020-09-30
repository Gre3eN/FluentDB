# FluentDB
A fluent API for database queries in C#

This is a work-in-progress version of FluentDB.

## Purpose
This API can be used for easy access to any database. It provides a set of tools to configure database settings and build native queries with a fluent builder approach.
By not enforcing the coupling of database and backend entities, this framework works best while dealing with a legacy database. Instead of having to inherit the
flaws of the database design within the entities, the native approach leaves a lot of freedom for designing backend entities. 
