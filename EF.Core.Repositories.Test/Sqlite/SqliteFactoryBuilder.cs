using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sqlite
{
    public class SqliteFactoryBuilder<TContext> : IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        private readonly Func<TContext, CancellationToken, Task> _contextAction;

        public SqliteFactoryBuilder(Action<TContext> contextAction) : this(async (ctx, token) => await Task.Run(() => contextAction(ctx), token))
        { }

        public SqliteFactoryBuilder(Func<TContext, Task> contextAction) : this(async (ctx, _) => await contextAction(ctx))
        { }

        public SqliteFactoryBuilder(Func<TContext, CancellationToken, Task> contextAction)
        {
            _contextAction = contextAction;
        }

        public async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = new SqliteRepositoryFactory<TContext>();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await _contextAction(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return factory;
        }
    }
}