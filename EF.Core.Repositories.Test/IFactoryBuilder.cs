using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    public interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        public static IFactoryBuilder<TContext> InMemory(Func<IEnumerable<object>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        public static IFactoryBuilder<TContext> InMemory(Func<Task<IEnumerable<object>>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        public static IFactoryBuilder<TContext> InMemory(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<IEnumerable<object>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<IEnumerable<object>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<IEnumerable<object>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<Task<IEnumerable<object>>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}