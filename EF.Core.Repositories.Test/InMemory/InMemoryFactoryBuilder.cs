using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.InMemory
{
    public class InMemoryFactoryBuilder<TContext> : IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        private readonly Func<TContext, CancellationToken, Task> _contextAction;

        public InMemoryFactoryBuilder(Action<TContext> contextAction) : this(async (ctx, token) => await Task.Run(() => contextAction(ctx), token))
        { }

        public InMemoryFactoryBuilder(Func<TContext, Task> contextAction) : this(async (ctx, _) => await contextAction(ctx))
        { }

        public InMemoryFactoryBuilder(Func<TContext, CancellationToken, Task> contextAction)
        {
            _contextAction = contextAction;
        }

        public async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = new InMemoryRepositoryFactory<TContext>();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await _contextAction(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return factory;
        }
    }
}