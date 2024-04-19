# EF.Core.Repositories

Extends Entity Framework Core to provide a Repository Pattern based on LINQ.

## Installing
Install NuGet package from NuGet.org

```sh
$ dotnet add package EF.Core.Repositories
```

## Requirements
- .NET 7 or higher
- Entity Framework Core 7.0.14+

## Getting Started
The Repository Factory can be registered by calling ConfigureData() extension.

The Action&lt;DbContextOptionsBuilder&gt; will be passed to AddDbContextFactory() extension provided by Entity Framework Core.

*Note* Due to the support for multiple asynchronous actions being executed against one or more repositories, multiple contexts may be created, and leveraging a DbContextFactory is required.
```csharp
using EF.Core.Repositories;
...
services.ConfigureData<TContext>(opt => ...);
```

Within a Service/Controller/etc.
```csharp
public class Consumer
{
  private readonly IRepositoryFactory<TContext> _factory;

  public Consumer(IRepositoryFactory<TContext> factory)
  {
    _factory = factory;
  }

  ...

  public async Task Command()
  {
    var repository = _factory.GetRepository<TEntity>();
    ...
  }
}
```

## Usage

### IReadOnlyRepository LINQ Extensions
The majority of IQueryable Extensions are also available for IReadOnlyRepository.
- Distinct()
- Except()
- GroupBy()
- Intersect()
- IntersectBy()
- Join()
- OrderBy()
- OrderByDescending()
- Select()
- SelectMany()
- Skip()
- SkipLast()
- SkipWhile()
- Take()
- TakeLast()
- TakeWhile()
- ThenBy()
- ThenByDescending()
- Union()
- UnionBy()
- Where()
- Zip()

### IReadOnlyRepository Async Extensions
Additional Extensions that will enumerate the Repository against the DbContext.
- AllAsync()
- AnyAsync()
- AverageAsync()
- CountAsync()
- FirstAsync()
- FirstOrDefaultAsync()
- GetAsync()
  - *Returns an IEnumerable containing the entities of the Repository.*
- LastAsync()
- LastOrDefaultAsync()
- LongCountAsync()
- MaxAsync()
- MinAsync()
- SingleAsync()
- SingleOrDefaultAsync()
- SumAsync()

### IRepository Extensions
IRepository has all the extensions available to IReadOnlyRepository and adds the following.
- Include()
- ThenInclude()

#### Include()
Just like when working with a DbSet exposed by a DbContext, you can include loading navigation properties of your entities.

Ex. Retrieving all Users, their UserRoles, and the subsequent referenced Role
```csharp
var repository = _factory
  .GetRepository<User>()
  .Include(x => x.UserRoles)
  .ThenInclude(x => x.Role);

var users = await repository.GetAsync();
```

### IRepository Async Extensions
IRepository has all the async extensions available to IReadOnlyRepository and adds the following.
- DeleteAsync(entity)
- DeleteByIdAsync(key)
- GetAsync(key)
- InsertAsync(entity)
- UpdateAsync(entity)

#### GetAsync(key) and DeleteByIdAsync(key)
These functions allow you to retreive/remove an entity based upon primary key data.

Ex. Retrieving a User with a simple primary key of (int UserId)
```csharp
var repository = _factory.GetRepository<User>();

var user = await repository.GetAsync(new { UserId = 5 });
```

Ex. Retrieving a UserRole with a complex primary key of (int UserId, int RoleId)
```csharp
var repository = _factory.GetRepository<UserRole>();

var userrole = await repository.GetAsync(new { UserId = 5, RoleId = 1 });
```

#### UpdateAsync(entity)
By default UpdateAsync will ignore all navigation properties and only update the concrete data fields within the entity.

To include a navigation property as part of the update use the Include() extension.

Ex. Adding a UserRole to a User
```csharp
var repository = _factory.GetRepository<User>();

var user = await repository
  .Include(x => x.UserRoles)
  .GetAsync(new { UserId = 5 });

user.UserRoles.Add(
  new UserRole
  {
    UserId = 5,
    RoleId = 2,
    ...
  });

var result = await repository
  .Include(x => x.UserRoles)
  .UpdateAsync(user);
```

### Transactions
Normally calls to InsertAsync or UpdateAsync immediately call SaveChangesAsync on the backing context and commit the changeset to the database.

Transactions allow for multiple changes to be committed to the database within a single SaveChangesAsync.

#### Example
Inserting multiple users.
```csharp
using var transaction = _factory.CreateTransaction();

var repository = transaction.GetRepository<User>();

foreach (var user in users)
{
    await repository.InsertAsync(user);
}

var inserted = await transaction.CommitAsync();
```

## Testing
The EF.Core.Repositories.Test package can be used to facilitate testing.

### Supported Providers
Currently there are three data providers supported.
- InMemory
    - Utilizes the EntityFrameworkCore.InMemory provider
- Sqlite
    - Utilizes the EntityFrameworkCore.Sqlite provider with in memory Sqlite instances
- Sql (*Recommended*)
    - Utilizes the EntityFrameworkCore.SqlServer provider
    - A connection string must be passed to the builder method for use during tests
        - Initial Catalog parameter will be ignored if included
    - User must have create database permissions on the server
    - Each time CreateFactoryAsync() is called a new database on configured server is created
    - Sql Factories will automatically delete the created database when disposed.
    - Failed tests will not trigger .Dispose() when factory is wrapped in a using context.
        - This will cause that database to survive and be available to aid in debugging test failure.

### FactoryBuilders
Each provider has an IFactoryBuilder&lt;T&gt; which uses a seed function to specify all objects that should be stored in the data source when it is initialized.

Best practice is to initialize a builder in your constructor and then create a factory to start each test.  This will result in each test using its own data source.

### Example
Testing a UserController's Add/Update endpoints

```csharp
public class UserControllerTests
{
    private readonly IFactoryBuilder<MyContext> _builder;

    public UserControllerTests()
    {
        _builder = IFactoryBuilder<MyContext>.Sql("Server=(local);Integrated Security=true;TrustServerCertificate=true",
            () => new object[]
            {
                new User
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "user@test.com",
                }
            });
    }

    [Fact]
    public async void AddUser()
    {
        using var factory = await _builder.CreateFactoryAsync();

        var controller = new UserController(factory);

        var user = new User
        {
            Id = 2,
            Name = "Test User 2",
            Email = "user2@test.com",
        };

        var result = await controller.Add(user);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async void UpdateUser()
    {
        using var factory = await _builder.CreateFactoryAsync();

        var controller = new UserController(factory);

        var user = new User
        {
            Id = 1,
            Name = "Test User 1",
            Email = "user1@test.com",
        };

        var result = await controller.Update(user);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

}
```
