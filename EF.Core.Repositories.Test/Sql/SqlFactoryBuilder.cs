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

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Func<IEnumerable<object>> entityFunction) : base(entityFunction)
        {
            _builder = builder;
        }

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Func<Task<IEnumerable<object>>> entityFunction) : base(entityFunction)
        {
            _builder = builder;
        }

        public SqlFactoryBuilder(SqlConnectionStringBuilder builder, Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) : base(entityFunction)
        {
            _builder = builder;
        }

        public override async Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default)
        {
            var factory = GetFactory();
            using var context = await factory.GetDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.SeedDataAsync(await _entityFunction(cancellationToken), cancellationToken);
            return factory;
        }

        protected override IRepositoryFactory<TContext> GetFactory() => new SqlRepositoryFactory<TContext>(_builder);
    }
}