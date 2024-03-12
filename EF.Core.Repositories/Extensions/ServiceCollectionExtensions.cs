using EF.Core.Repositories.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Core.Repositories.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureData<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = default)
            where TContext : DbContext
        {
            return services
                .AddDbContextFactory<TContext>(optionsAction)
                .AddScoped<IRepositoryFactory<TContext>, RepositoryFactory<TContext>>();
        }
    }
}