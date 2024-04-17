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
        public static IFactoryBuilder<TContext> InMemory(Func<IEnumerable<object>> entityFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(entityFunction);

        public static IFactoryBuilder<TContext> InMemory(Func<Task<IEnumerable<object>>> entityFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(entityFunction);

        public static IFactoryBuilder<TContext> InMemory(Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(entityFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<IEnumerable<object>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, entityFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<Task<IEnumerable<object>>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, entityFunction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, entityFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<IEnumerable<object>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), entityFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<Task<IEnumerable<object>>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), entityFunction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), entityFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<IEnumerable<object>> entityFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(entityFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<Task<IEnumerable<object>>> entityFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(entityFunction);

        public static IFactoryBuilder<TContext> Sqlite(Func<CancellationToken, Task<IEnumerable<object>>> entityFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(entityFunction);

        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}