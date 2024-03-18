using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories
{
    /// <summary>
    /// A factory for creating <see cref="IRepository{T}"/>.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a new <see cref="ITransaction"/>.
        /// </summary>
        /// <returns>The created <see cref="ITransaction"/>.</returns>
        ITransaction CreateTransaction();

        /// <summary>
        /// Creates a new <see cref="DbContext"/> instance in an async context.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created context.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        Task<DbContext> GetDbContextAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a readonly repository for accessing data of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data in the repository.</typeparam>
        /// <returns>An <see cref="IReadOnlyRepository{T}"/>.</returns>
        IReadOnlyRepository<T> GetReadOnlyRepository<T>() where T : class;

        /// <summary>
        /// Gets a read/write repository for accessing/updating data of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data in the repository.</typeparam>
        /// <returns>An <see cref="IRepository{T}"/>.</returns>
        IRepository<T> GetRepository<T>() where T : class;
    }

    /// <summary>
    /// A factory for creating <see cref="IRepository{T}"/> based upon a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of <see cref="DbContext"/> to be created by the factory.
    /// </typeparam>
    public interface IRepositoryFactory<TContext> : IRepositoryFactory
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a new <typeparamref name="TContext"/> instance in an async context.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created context.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        new Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default);
    }
}