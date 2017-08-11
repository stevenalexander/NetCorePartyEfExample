# .NET Core Party EF example

![BuildStatus](https://travis-ci.org/stevenalexander/NetCorePartyEfExample.svg?branch=master)

Example solution using .NET Core EF with a split class library for migrations used by a Web application to interact with a Database.

This is intended as an example of implementing and using the [universal person and organisation data model](http://tdan.com/a-universal-person-and-organization-data-model/5014) created by Martin Fowler. This model allows you to record links to parties regardless of whether they are persons or organisations (or other), a common requirement in business operations dealing with interactions from different types of actors.

## Setup Database

Update the database connection string in `PartyData/appsettings.json`.

In Visual Studio:
1. Open the solution in Visual Studio
2. Open "Package manager console" and select the project "PartyData"
3. Enter the command "Update-Database"

In Powershell/VS Code terminal
1. In the shell navigate to path of the PartyData project
2. Enter the command "dotnet restore"
3. Enter the command "dotnet ef database update"

Powershell/VS Code requires csproj file contains:

```
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="1.0.1" />
  </ItemGroup>
```

## Notes

- EF Core currently [doesn't support Table per Type](https://github.com/aspnet/EntityFramework/issues/2266), so avoid Entities that inherit from other Entities, as it will generate large flat tables. Instead model the Entities based on the table structure you intend and create repository/query objects that map/manage those Entities to Business objects that contain and manipulate data from multiple entities.
- Travis CI supports [csharp net core builds](https://docs.travis-ci.com/user/languages/csharp/), but the dotnet framework version property does not directly match the framework name, i.e. .NETCoreApp 1.1 is `dotnet: 1.0.1`.
- In memory DB does not like EF setting default values on model maps and will return wrong values. Avoid defaults in context like `.HasDefaultValue(true)` and set in constructors or respositories.

## Links

- http://www.c-sharpcorner.com/article/crud-operations-in-asp-net-core-using-entity-framework-core-code-first/
- https://www.benday.com/2017/02/17/ef-core-migrations-without-hard-coding-a-connection-string-using-idbcontextfactory/
