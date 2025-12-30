using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test.Sqlite
{
    internal class SqliteFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        protected override IRepositoryFactory<TContext> GetFactory() => new SqliteRepositoryFactory<TContext>();
    }
}