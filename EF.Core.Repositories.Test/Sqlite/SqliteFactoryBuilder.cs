using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sqlite
{
    internal class SqliteFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        public SqliteFactoryBuilder(Action<TContext> contextAction) : base(contextAction)
        { }

        public SqliteFactoryBuilder(Func<TContext, Task> contextAction) : base(contextAction)
        { }

        public SqliteFactoryBuilder(Func<TContext, CancellationToken, Task> contextAction) : base(contextAction)
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new SqliteRepositoryFactory<TContext>();
    }
}