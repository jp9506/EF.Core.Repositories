using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Base
{
    internal abstract class FactoryBuilderBase<TContext> : IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        protected readonly Func<TContext, CancellationToken, Task> _contextAction;

        public FactoryBuilderBase(Action<TContext> contextAction) : this(async (ctx, token) => await Task.Run(() => contextAction(ctx), token))
        { }

        public FactoryBuilderBase(Func<TContext, Task> contextAction) : this(async (ctx, _) => await contextAction(ctx))
        { }

        public FactoryBuilderBase(Func<TContext, CancellationToken, Task> contextAction)
        {
            _contextAction = contextAction;
        }

        public virtual async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = GetFactory();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await _contextAction(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return factory;
        }

        protected abstract IRepositoryFactory<TContext> GetFactory();
    }
}