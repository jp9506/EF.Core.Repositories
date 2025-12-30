using EF.Core.Repositories.Test.Base;
using EF.Core.Repositories.Test.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sql.Instance
{
    internal class SqlFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        protected override IRepositoryFactory<TContext> GetFactory() => new SqlRepositoryFactory<TContext>(GetConnectionString());

        protected override async Task SeedDataAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (GetSeed != null)
                await context.SeedDataAsync(await GetSeed(cancellationToken), cancellationToken);
        }
    }
}