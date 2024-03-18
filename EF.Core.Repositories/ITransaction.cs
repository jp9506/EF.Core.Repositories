using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories
{
    /// <summary>
    /// A Thread-safe container for supporting multiple changes within a single Db transaction
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Commits all changes in <see cref="DbContext"/> to database.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IEnumerable{T}"/> of all entities that effected by the transaction.
        /// </returns>
        Task<IEnumerable<object>> CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="DbContext"/> instance in an async context.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the context.
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

    internal interface IInternalTransaction : ITransaction
    {
        bool AutoCommit { get; }
    }
}