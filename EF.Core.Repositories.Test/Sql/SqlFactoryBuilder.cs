using EF.Core.Repositories.Test.Base;
using EF.Core.Repositories.Test.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sql
{
    internal class SqlFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        private readonly SqlConnectionStringBuilder _builder;

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Action<TContext> contextAction) : base(contextAction)
        {
            _builder = builder;
        }

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Func<TContext, Task> contextAction) : base(contextAction)
        {
            _builder = builder;
        }

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Func<TContext, CancellationToken, Task> contextAction) : base(contextAction)
        {
            _builder = builder;
        }

        public override async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = GetFactory();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await _contextAction(context, cancellationToken);
            await SaveChangesAsync(factory, context.ChangeTracker.Entries(), cancellationToken);
            return factory;
        }

        protected override IRepositoryFactory<TContext> GetFactory() => new SqlRepositoryFactory<TContext>(_builder);

        private async Task SaveChangesAsync(IRepositoryFactory<TContext> factory, IEnumerable<EntityEntry> source, CancellationToken cancellationToken)
        {
            var data = source.GroupBy(x => x.Metadata);
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            await context.DisableForeignKeyContraintsAsync(cancellationToken);
            foreach (var set in data)
            {
                await context.EnableIdentityInsertAsync(set.Key, cancellationToken);
                foreach (var e in set)
                {
                    await context.InsertEntityAsync(e, cancellationToken);
                }
                await context.DisableIdentityInsertAsync(set.Key, cancellationToken);
            }
            await context.EnableForeignKeyContraintsAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}