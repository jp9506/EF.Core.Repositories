using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test.Sql.Instance
{
    internal class SqlRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        public SqlRepositoryFactory(string connString)
        {
            _options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(connString)
                .Options;
        }

        protected override void OnDisposing()
        {
            var ctx = GetDbContextAsync().Result;
            ctx.Database.EnsureDeleted();
        }
    }
}