using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Base
{
    internal abstract class FactoryBuilderBase<TContext> : IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        public FactoryBuilderBase()
        { }

        internal Func<string> GetConnectionString { get; set; } = () => "";
        internal Func<CancellationToken, Task<IEnumerable<object>>>? GetSeed { get; set; }

        public async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = GetFactory();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await SeedDataAsync(context);
            return factory;
        }

        protected abstract IRepositoryFactory<TContext> GetFactory();

        protected virtual async Task SeedDataAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (GetSeed != null)
                await context.AddRangeAsync(await GetSeed(cancellationToken));
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}