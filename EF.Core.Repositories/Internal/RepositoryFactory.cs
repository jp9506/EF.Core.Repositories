using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        public ITransaction CreateTransaction()
        {
            return new Transaction(this);
        }

        async Task<DbContext> IRepositoryFactory.GetDbContextAsync(CancellationToken cancellationToken)
        {
            return await GetDbContextAsync(cancellationToken);
        }

        public async Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            return await _contextFactory.CreateDbContextAsync(cancellationToken);
        }

        public IReadOnlyRepository<T> GetReadOnlyRepository<T>()
            where T : class
        {
            var transaction = new AutoTransaction(this);
            return transaction.GetReadOnlyRepository<T>();
        }

        public IRepository<T> GetRepository<T>()
            where T : class
        {
            var transaction = new AutoTransaction(this);
            return transaction.GetRepository<T>();
        }

        private sealed class AutoTransaction : TransactionBase
        {
            public AutoTransaction(IRepositoryFactory factory) : base(factory)
            {
            }

            public override bool AutoCommit => true;
        }

        private sealed class Transaction : TransactionBase
        {
            public Transaction(IRepositoryFactory factory) : base(factory)
            {
            }

            public override bool AutoCommit => false;
        }
    }
}