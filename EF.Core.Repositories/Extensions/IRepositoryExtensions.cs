using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// LINQ extension methods for <see cref="IRepository{T}"/>.
    /// </summary>
    public static class IRepositoryExtensions
    {
        #region Delete

        /// <summary>
        /// Delete an entity from a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> to delete an <paramref name="entity"/> from.
        /// </param>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if the delete was successful, <see langword="false"/> if no entity was deleted.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. A concurrency
        /// violation occurs when an unexpected number of rows are affected during save. This is
        /// usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<bool> DeleteAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, t) =>
            {
                var ctx = await t.GetDbContextAsync(cancellationToken);
                ctx.Remove(entity);
                if (t.AutoCommit)
                {
                    var res = await t.CommitAsync(cancellationToken);
                    return res.Any();
                }
                return true;
            });

        /// <summary>
        /// Delete an entity from a repository based upon the specified key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="key"/> must be an object containing properties that match the primary
        /// key structure of <typeparamref name="T"/>.
        /// </para>
        /// <para>For an entity whose primary key is a single column (Id). Use new { Id = ? }.</para>
        /// <para>
        /// For an entity whose primary key has multiple columns (Id1, Id2,...). Use new { Id1 = ?,
        /// Id2 = ?,... }.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to delete an entity from.</param>
        /// <param name="key">An object specifying all primary key fields.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if the delete was successful, <see langword="false"/> if no entity was deleted.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. A concurrency
        /// violation occurs when an unexpected number of rows are affected during save. This is
        /// usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<bool> DeleteByIdAsync<T>(this IRepository<T> repository, object key, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, t) =>
            {
                var ctx = await t.GetDbContextAsync(cancellationToken);
                var entity = await r.GetAsync(ctx, key, cancellationToken);
                if (entity == null) return false;
                ctx.Remove(entity);
                if (t.AutoCommit)
                {
                    var res = await t.CommitAsync(cancellationToken);
                    return res.Any();
                }
                return true;
            });

        #endregion Delete

        #region Update

        /// <summary>
        /// Update an entity from a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to update the <paramref name="entity"/>.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated entity.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. A concurrency
        /// violation occurs when an unexpected number of rows are affected during save. This is
        /// usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> UpdateAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, t) =>
            {
                var ctx = await t.GetDbContextAsync(cancellationToken);
                var current = await r.GetAsync(ctx, entity, cancellationToken);
                if (current == null) return default;
                ctx.Entry(current).CurrentValues.SetValues(entity);
                await r.HandleExpressionUpdateAsync(ctx, current, entity, cancellationToken);
                if (t.AutoCommit)
                {
                    var res = await t.CommitAsync(cancellationToken);
                    return res.Any() ? current : default;
                }
                return current;
            });

        #endregion Update

        #region Insert

        /// <summary>
        /// Insert an entity into a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to insert the <paramref name="entity"/>.</param>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the inserted entity.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. A concurrency
        /// violation occurs when an unexpected number of rows are affected during save. This is
        /// usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> InsertAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, t) =>
            {
                var ctx = await t.GetDbContextAsync(cancellationToken);
                var e = ctx.Attach(entity);
                e.State = EntityState.Added;
                if (t.AutoCommit)
                {
                    var res = await t.CommitAsync(cancellationToken);
                    return res.Any() ? e.Entity : default;
                }
                return e.Entity;
            });

        #endregion Insert

        #region Execute

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalRepository<T> repository, Func<IInternalRepository<T>, IInternalTransaction, Task<TResult>> expression)
            where T : class
        {
            return await expression(repository, repository.Transaction);
        }

        #endregion Execute
    }
}