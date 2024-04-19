using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Sql
{
    internal class SqlRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        public SqlRepositoryFactory(SqlConnectionStringBuilder builder)
        {
            var id = Guid.NewGuid().ToString().ToLower().Replace('-', '.');
            builder.InitialCatalog = $"{typeof(TContext).Name}.{id}";
            _options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(builder.ConnectionString)
                .Options;
        }

        protected override void OnDisposing()
        {
            var ctx = GetDbContextAsync().Result;
            ctx.Database.EnsureDeleted();
        }
    }
}