using EF.Core.Repositories.Test.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.MsSql;

namespace EF.Core.Repositories.Test.Sql.Container
{
    internal class SqlRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        public SqlRepositoryFactory(MsSqlContainer host)
        {
            var builder = new SqlConnectionStringBuilder(host.GetConnectionString());
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