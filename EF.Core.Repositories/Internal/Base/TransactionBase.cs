using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class TransactionBase(IRepositoryFactory factory) : IInternalTransaction
    {
        private readonly IRepositoryFactory _factory = factory;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool disposedValue;
        public abstract bool AutoCommit { get; }
        private DbContext? DbContext { get; set; } = null;

        public virtual async Task<IEnumerable<object>> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (DbContext != null)
                {
                    DbContext.ChangeTracker.DetectChanges();
                    var entities = DbContext.ChangeTracker.Entries()
                        .Where(x =>
                            x.State == EntityState.Deleted ||
                            x.State == EntityState.Added ||
                            x.State == EntityState.Modified)
                        .Select(x => x.Entity).ToArray();
                    var count = await DbContext.SaveChangesAsync(cancellationToken);
                    if (count > 0)
                        return entities;
                }
            }
            finally { _semaphore.Release(); }
            return Enumerable.Empty<object>();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<DbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (DbContext == null)
                {
                    DbContext = await _factory.GetDbContextAsync(cancellationToken);
                    DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                }
            }
            finally { _semaphore.Release(); }
            return DbContext;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DbContext?.Dispose();
                    _semaphore.Dispose();
                }

                disposedValue = true;
            }
        }

        private sealed class ReadOnlyRepository<T>(IInternalTransaction transaction) : ReadOnlyRepositoryBase<T>(transaction)
            where T : class
        {
            public override IQueryable<T> EntityQuery(DbContext context) => context.Set<T>().AsQueryable();
        }

        private sealed class Repository<T>(IInternalTransaction transaction) : RepositoryBase<T>(transaction)
            where T : class
        {
            public override IQueryable<T> EntityQuery(DbContext context) => context.Set<T>().AsQueryable();

            public override Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
    }
}