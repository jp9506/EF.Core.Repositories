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

The Action<DbContextOptionsBuilder> will be passed to AddDbContextFactory() extension provided by Entity Framework Core.

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
- DeleteRangeAsync(entities)
- DeleteRangeByIdAsync(keys)
- GetAsync(key)
- InsertAsync(entity)
- InsertRangeAsync(entities)
- UpdateAsync(entity)
- UpdateRangeAsync(entities)

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
