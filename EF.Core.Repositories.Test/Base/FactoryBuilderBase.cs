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
        protected readonly Func<CancellationToken, Task<IEnumerable<object>>> _seedFunction;

        public FactoryBuilderBase(Func<IEnumerable<object>> seedFunction) : this(async token => await Task.Run(() => seedFunction(), token))
        { }

        public FactoryBuilderBase(Func<Task<IEnumerable<object>>> seedFunction) : this(async _ => await seedFunction())
        { }

        public FactoryBuilderBase(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction)
        {
            _seedFunction = seedFunction;
        }

        public virtual async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = GetFactory();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.AddRangeAsync(await _seedFunction(cancellationToken));
            await context.SaveChangesAsync(cancellationToken);
            return factory;
        }

        protected abstract IRepositoryFactory<TContext> GetFactory();
    }
}