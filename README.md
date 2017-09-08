# .NET Core Party EF example

![BuildStatus](https://travis-ci.org/stevenalexander/NetCorePartyEfExample.svg?branch=master)

![data model](https://raw.githubusercontent.com/stevenalexander/NetCorePartyEfExample/master/Images/party-model.png "data model")

Example solution using .NET Core EF with a split class library for migrations used by a Web application to interact with a Database.

This is intended as an example of implementing and using the [universal person and organisation data model](http://tdan.com/a-universal-person-and-organization-data-model/5014) created by Martin Fowler. This model allows you to record links to parties regardless of whether they are persons or organisations (or other), a common requirement in business operations dealing with interactions from different types of actors.

This project demonstrates:

- Implementing the Party/Organisation/Person data model using Entity Framework in .NET Core
- Using Code First approach to generate migration scripts in a separate class library for controlled release of database changes
- Ability to execute migration scripts with a configurable connection string outside of web code, allowing migration DB user with higher permissions than web DB user 
- Complex queries against multiple Entities using Linq
- Testing Entity Framework queries against a SQLite DB for unit tests (as close as possible) of respository objects
- Repository pattern with injected DbContext, separating Web controller logic from DbContext for easy testing with mocks/subsitutes
- Repositories which return paged/sorted results in a form which supports both [JQuery Datatables](https://datatables.net/) and non-Javascript paging
- Adding browser tests via node with wdio to unit test project

## Requires

- .NET Core framework
- Visual Studio or VS Code
- SQL Server Database (or Azure SQL)
- Node/npm (for browser tests)

## Run tests

Open powershell or VS Code terminal and navigate into the unit test projects directory:

```
dotnet test
```

### Browser tests

Open powershell or VS Code terminal and navigate into the web unit test projects directory:

```
npm install
npm run-script test-e2e
```

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

Adding a new migration in Visual Studio:
```
# will detect changes in source entities and generate based on condition of DB
Add-Migration MyNewChange
# will apply all outstanding migrations
Update-Database
# rollback to named migration
Update-Database -TargetMigration "MyPreviousChange"
```

## Paging and sorting pattern for non-Javascript and Datatables

I like [JQuery Datatables](https://datatables.net/). It's a easy to use JQuery plug-in that allows you to enhance an HTML table to support paging/sorting/filtering and all sorts of functionality with little configuration. It supports server side processing (something I've [blogged on before](https://stevenwilliamalexander.wordpress.com/2016/02/12/asp-mvc-datatables-server-side/)) to allow serving large datasets. 

But it has some issues. 

By default it isn't responsive and doesn't play nice with small screens, it's hard to style if you are using custom styling for your website and it will cause some accessibility issues. Also it needs Javascript, so sites that need to support no-js can't rely on it for paging large tables.

Since I like the Datatables command patterns for API calls and don't like duplicating logic, I've created this [sample project](https://github.com/stevenalexander/NetCorePartyEfExample) which shows how you can implement your model/view/controller logic and back-end logic to support serving a paged/sorted table both with Datatables and pure HTML GET requests on a page. This cuts down on the amount of logic needed and provides an easy to follow pattern for retrieving and using the paged data.

Even if you do not want to use Datatables it's always good to use an approach which will be familiar to other developers and have a pattern that encourages code reuse and consistency.

Here's the sample Person list using Datatables:

![person-list-js](https://raw.githubusercontent.com/stevenalexander/NetCorePartyEfExample/master/Images/person-datatables-js-enabled.PNG "person-list-js")

Here's the same page with Javascript disabled using HTML GET requests for paging/sorting:

![person-list](https://raw.githubusercontent.com/stevenalexander/NetCorePartyEfExample/master/Images/person-datatables-js-disabled.PNG "person-list")

Datatables provides the quick AJAX redraw of the table with enhanced paging/sorting functions, while the HTML GET provides the non-Javascript support.

To implement this I used a number of classes with generics/abstract methods to allow re-use for different pages/tables:

 [PagedSortedViewModel](https://github.com/stevenalexander/NetCorePartyEfExample/blob/master/WebApplicationParty/Models/PagedSortedViewModel.cs) - model that can be used for both JSON serialization in Datatable server-side and rendering HTML table. 

 ```
    public class PagedSortedViewModel<TData> : IPagedSortedViewModel
    {
        public int Draw { get; set; }
        ...
        public IEnumerable<TData> Data { get; set; }
        ...
    }
 ```

[PersonPagedSortedTableController](https://github.com/stevenalexander/NetCorePartyEfExample/blob/master/WebApplicationParty/Controllers/PersonPagedSortedTableController.cs) - controller with routes for both HTML GET and Datatables JSON call

```
    public class PersonPagedSortedTableController : Controller
    {
        ...
        [HttpGet]
        public async Task<IActionResult> Index(int start = 0, int length = 10, string orderColumn = "Name", bool orderAscending = true)
        {
            var model = await GetPagedSortedResultsAsViewModel(0, start, length, orderColumn, orderAscending);

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> DatatableJson(int draw = 0, int start = 0, int length = 10)
        {
            var isAscending = Request.Query["order[0][dir]"] == "asc";
            int columnIdentifier = Convert.ToInt32(Request.Query["order[0][column]"]);
            string orderColumnName = GetColumnName(columnIdentifier);

            var model = await GetPagedSortedResultsAsViewModel(draw, start, length, orderColumnName, isAscending);

            return Json(model);
        }

        private async Task<PagedSortedViewModel<PersonResultItem>> GetPagedSortedResultsAsViewModel(int draw, int start, int length, string orderColumn, bool orderAscending)
        {
            var result = await _pagedSortedRepository.GetPagedSortedResults(start, length, orderColumn, orderAscending);

            return new PagedSortedViewModel<PersonResultItem>
            {
                Draw = draw,
                ...
                Data = result.data,
            };
        }

        private string GetColumnName(int columnIdentifier)
        {
            switch (columnIdentifier)
            {
                case 0: return "Name";
                ...
            }

        }
    }
```

[AbstractPagedSortedRepository](https://github.com/stevenalexander/NetCorePartyEfExample/blob/master/PartyData/Repositories/AbstractPagedSortedRepository.cs) - abstract repository class that has a number of virtual and abstract methods, wiring together the queries needed to return the paged/sorted result set so that minimal custom logic is needed for each different table.

```
    public abstract class AbstractPagedSortedRepository<TResultItem> : IPagedSortedRepository<TResultItem>
    {
        public async Task<PagedSortedResult<TResultItem>> GetPagedSortedResults(int start, int length, string orderColumn, bool orderAscending)
        {
            var innerJoinQuery = GetQuery();

            var recordsTotal = await GetRecordsTotalQuery(innerJoinQuery).CountAsync();

            var whereQuery = GetWhereQuery(innerJoinQuery);

            var recordsFiltered = await GetRecordsFilteredQuery(whereQuery).CountAsync();

            var sortedWhereQuery = GetSortedWhereQuery(whereQuery, orderColumn, orderAscending);

            var pagedSortedWhereQuery = sortedWhereQuery.Skip(start).Take(length);

            var data = await pagedSortedWhereQuery.ToListAsync();

            return new PagedSortedResult<TResultItem>
            {
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data,
            };
        }
    }
```

[PersonPagedSortedRepository](https://github.com/stevenalexander/NetCorePartyEfExample/blob/master/PartyData/Repositories/PersonPagedSortedRepository.cs) - Implementation of the abstract repository for a table showing joined results of the Person/Party entities.

```
public class PersonPagedSortedRepository : AbstractPagedSortedRepository<PersonResultItem>
    {
        ...
        protected override IQueryable<PersonResultItem> GetQuery()
        {
            return from p in _partyDbContext.Parties
                   join o in _partyDbContext.Persons on p.PartyId equals o.PartyId
                   select new PersonResultItem { PartyId = p.PartyId, Name = p.Name, EmailAddress = o.EmailAddress, DateOfBirth = o.DateOfBirth, DateCreated = p.DateCreated };
        }

        protected override IQueryable<PersonResultItem> GetSortedWhereQuery(IQueryable<PersonResultItem> whereQuery, string orderColumn, bool orderAscending)
        {
            switch (orderColumn)
            {
                case "Name": return orderAscending ? whereQuery.OrderBy(x => x.Name) : whereQuery.OrderByDescending(x => x.Name);
                ...
                default: return whereQuery;
            }
        }
    }
```

The [view](https://github.com/stevenalexander/NetCorePartyEfExample/blob/master/WebApplicationParty/Views/PersonPagedSortedTable/Index.cshtml) renders the table, and has Javascript to use Datatables if Javascript is enabled (hiding HTML paging/sorting controls).

## Notes

- EF Core currently [doesn't support Table per Type](https://github.com/aspnet/EntityFramework/issues/2266), so avoid Entities that inherit from other Entities, as it will generate large flat tables. Instead model the Entities based on the table structure you intend and create repository/query objects that map/manage those Entities to Business objects that contain and manipulate data from multiple entities.
- Travis CI supports [csharp net core builds](https://docs.travis-ci.com/user/languages/csharp/), but the dotnet framework version property does not directly match the framework name, i.e. .NETCoreApp 1.1 is `dotnet: 1.0.1`.
- In memory DB does not like EF setting default values on model maps and will return wrong values. Avoid defaults in context like `.HasDefaultValue(true)` and set in constructors or respositories.
- SpecFlow does not currently support .NET Core, so used Node wdio to create browser tests.
- Running gulp task for wdio tests using Visual Studio Task Runner Explorer can give error for `node-fiber`, use VS Code terminal or Powershell.

## Links

- http://www.c-sharpcorner.com/article/crud-operations-in-asp-net-core-using-entity-framework-core-code-first/
- https://www.benday.com/2017/02/17/ef-core-migrations-without-hard-coding-a-connection-string-using-idbcontextfactory/
- https://docs.microsoft.com/en-us/ef/core/modeling/relationships
- https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-grouped-joins