using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Internal
{
    internal class RepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _contextFactory;

        public RepositoryFactory(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        async Task<DbContext> IRepositoryFactory.CreateDbContextAsync(CancellationToken cancellationToken)
        {
            return await CreateDbContextAsync(cancellationToken);
        }

        public async Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return await _contextFactory.CreateDbContextAsync(cancellationToken);
        }

        public IReadOnlyRepository<T> GetReadOnlyRepository<T>()
            where T : class
        {
            return new ReadOnlyRepository<T>(this);
        }

        public IRepository<T> GetRepository<T>()
                    where T : class
        {
            return new Repository<T>(this);
        }

        private sealed class ReadOnlyRepository<T> : ReadOnlyRepositoryBase<T>
            where T : class
        {
            public ReadOnlyRepository(IRepositoryFactory factory) : base(factory)
            {
            }

            public override IQueryable<T> EntityQuery(DbContext context) => context.Set<T>().AsQueryable();
        }

        private sealed class Repository<T> : RepositoryBase<T>
                    where T : class
        {
            public Repository(IRepositoryFactory factory) : base(factory)
            {
            }

            public override IQueryable<T> EntityQuery(DbContext context) => context.Set<T>().AsQueryable();

            public override Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
    }
}