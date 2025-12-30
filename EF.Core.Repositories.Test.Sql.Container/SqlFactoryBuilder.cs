using EF.Core.Repositories.Test.Base;
using EF.Core.Repositories.Test.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace EF.Core.Repositories.Test.Sql.Container
{
    internal class SqlFactoryBuilder<TContext> : ContainerFactoryBuilderBase<TContext, MsSqlBuilder, MsSqlContainer, MsSqlConfiguration>
        where TContext : DbContext
    {
        protected override IRepositoryFactory<TContext> GetFactory() => new SqlRepositoryFactory<TContext>(GetHost());

        protected override async Task SeedDataAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (GetSeed != null)
                await context.SeedDataAsync(await GetSeed(cancellationToken), cancellationToken);
        }
    }
}