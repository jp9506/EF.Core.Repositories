using Cocona;
using Demo.Data;
using EF.Core.Repositories;
using EF.Core.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo;

internal class Program
{
    public async Task Demo([FromService] IRepositoryFactory<DataContext> factory)
    {
        var userRepository = factory.GetRepository<User>();

        // Retrieve all entities within a repository
        _ = await userRepository
            .GetAsync();

        // Retrieve by Key
        _ = await userRepository
            .GetAsync(
                new
                {
                    Id = new Guid("A71D01DF-5685-4893-8A23-A5583BFE70B8")
                });

        // Retrieve using multipart key
        _ = await userRepository
            .GetAsync(
            new
            {
                Id1 = new Guid("..."),
                Id2 = new Guid("..."),
            });

        // Include Navigation Properties
        _ = await userRepository
            .Include(x => x.Roles)
            .GetAsync();

        // Insert/Update with Navigation Properties
        var u = new User();

        // ... Populate Properties

        // Insert User
        _ = await userRepository
            .Include(x => x.Roles)
            .InsertAsync(u);

        // Update User
        _ = await userRepository
            .Include(x => x.Roles)
            .UpdateAsync(u);

        // All Standard IEnumerable LINQ Extensions implemented
        _ = await userRepository
            .Include(x => x.Roles)
            .Where(x => x.Roles.Any(r => r.Id == 2))
            .GetAsync();

        _ = await userRepository
            .OrderBy(x => x.LastName)
            .GetAsync();

        _ = userRepository.Take(1);
    }

    private static void Main(string[] args)
    {
        CoconaApp.CreateHostBuilder()
            .ConfigureServices(services =>
            {
                services.ConfigureData<DataContext>(opt =>
                    opt.UseSqlServer());
            }).Run<Program>(args);
    }
}