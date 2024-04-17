using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    public interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        public static IFactoryBuilder<TContext> InMemory(Action<TContext> contextAction) => new InMemory.InMemoryFactoryBuilder<TContext>(contextAction);

        public static IFactoryBuilder<TContext> InMemory(Func<TContext, Task> contextAction) => new InMemory.InMemoryFactoryBuilder<TContext>(contextAction);

        public static IFactoryBuilder<TContext> InMemory(Func<TContext, CancellationToken, Task> contextAction) => new InMemory.InMemoryFactoryBuilder<TContext>(contextAction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Action<TContext> contextAction) => new Sql.SqlFactoryBuilder<TContext>(builder, contextAction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<TContext, Task> contextAction) => new Sql.SqlFactoryBuilder<TContext>(builder, contextAction);

        public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<TContext, CancellationToken, Task> contextAction) => new Sql.SqlFactoryBuilder<TContext>(builder, contextAction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Action<TContext> contextAction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), contextAction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<TContext, Task> contextAction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), contextAction);

        public static IFactoryBuilder<TContext> Sql(string connectionString, Func<TContext, CancellationToken, Task> contextAction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), contextAction);

        public static IFactoryBuilder<TContext> Sqlite(Action<TContext> contextAction) => new Sqlite.SqliteFactoryBuilder<TContext>(contextAction);

        public static IFactoryBuilder<TContext> Sqlite(Func<TContext, Task> contextAction) => new Sqlite.SqliteFactoryBuilder<TContext>(contextAction);

        public static IFactoryBuilder<TContext> Sqlite(Func<TContext, CancellationToken, Task> contextAction) => new Sqlite.SqliteFactoryBuilder<TContext>(contextAction);

        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}