using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sqlite
{
    internal class SqliteFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        public SqliteFactoryBuilder(Func<IEnumerable<object>> entityFunction) : base(entityFunction)
        { }

        public SqliteFactoryBuilder(Func<Task<IEnumerable<object>>> entityFunction) : base(entityFunction)
        { }

        public SqliteFactoryBuilder(Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) : base(entityFunction)
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new SqliteRepositoryFactory<TContext>();
    }
}