using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using Testcontainers.PostgreSql;

namespace EF.Core.Repositories.Test.PostgreSQL.Container
{
    internal class PostgreSQLRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        public PostgreSQLRepositoryFactory(PostgreSqlContainer host)
        {
            var builder = new NpgsqlConnectionStringBuilder(host.GetConnectionString());
            var id = Guid.NewGuid().ToString().ToLower().Replace('-', '.');
            builder.Database = $"{typeof(TContext).Name}.{id}";

            _options = new DbContextOptionsBuilder<TContext>()
                .UseNpgsql(builder.ConnectionString)
                .Options;
        }

        protected override void OnDisposing()
        {
            var ctx = GetDbContextAsync().Result;
            ctx.Database.EnsureDeleted();
        }
    }
}