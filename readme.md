# ReadRoulette

An API built on .NET 8 serving as the back end for the ReadRoulette platform. ReadRoulette allows users to create accounts.

## Purpose

* Users can create accounts in the platform
* Users can create a to-read book list
* Users can visit their to-read list and ask the platform for a random book
* Users can create book clubs and invite other users to the club
* The book club chooses a user to pick a book for the club to read

## Requirements

- .NET 8
- Postgresql

## How to run

1. Clone repo
2. `cd` into `ReadRoulette/ReadRoulette`
3. Run `dotnet user-secrets init`
4. Run `dotnet user-secrets set ConnectionStrings:DbConnectionString "Server=<db-server-here>;Database=ReadRoulette_Dev;Username=<user>;Password=<pw>"`
5. Run `dotnet ef database update`
6. Run `dotnet run`
7. Import `ReadRoulette.postman_collection.json` into Postman
8. Create a book in Postman.