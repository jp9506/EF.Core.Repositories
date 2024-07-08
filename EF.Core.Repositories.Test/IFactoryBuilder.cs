using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    /// <summary>
    /// A builder for creating <see cref="IRepositoryFactory{TContext}"/> based upon a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of <see cref="DbContext"/> to be created by the factory.
    /// </typeparam>
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.InMemory provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> InMemory(Func<IEnumerable<object>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.InMemory provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> InMemory(Func<Task<IEnumerable<object>>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.InMemory provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> InMemory(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new InMemory.InMemoryFactoryBuilder<TContext>(seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="builder">
        ///// A <see cref="SqlConnectionStringBuilder"/> to be used to create backing databases.
        ///// </param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<IEnumerable<object>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="builder">
        ///// A <see cref="SqlConnectionStringBuilder"/> to be used to create backing databases.
        ///// </param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="builder">
        ///// A <see cref="SqlConnectionStringBuilder"/> to be used to create backing databases.
        ///// </param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(SqlConnectionStringBuilder builder, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(builder, seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="connectionString">A connection string to be used to create backing databases.</param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(string connectionString, Func<IEnumerable<object>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="connectionString">A connection string to be used to create backing databases.</param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(string connectionString, Func<Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        ///// EntityFrameworkCore.SqlServer provider.
        ///// </summary>
        ///// <param name="connectionString">A connection string to be used to create backing databases.</param>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sql(string connectionString, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sql.SqlFactoryBuilder<TContext>(new SqlConnectionStringBuilder(connectionString), seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.Sqlite provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sqlite(Func<IEnumerable<object>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.Sqlite provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sqlite(Func<Task<IEnumerable<object>>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        ///// <summary>
        ///// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.Sqlite provider.
        ///// </summary>
        ///// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        ///// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        //public static IFactoryBuilder<TContext> Sqlite(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) => new Sqlite.SqliteFactoryBuilder<TContext>(seedFunction);

        /// <summary>
        /// Creates a <see cref="IRepositoryFactory{TContext}"/> for use in a test.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The created <see cref="IRepositoryFactory{TContext}"/>.</returns>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}