using EF.Core.Repositories.Internal;
using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Extensions
{
    public static class IRepositoryExtensions
    {
        #region Get

        /// <summary>
        /// Retrieves an entity from a repository based upon the specified key.
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
        /// <param name="repository">An <see cref="IRepository{T}"/> to retrieve an entity from.</param>
        /// <param name="key">An object specifying all primary key fields.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the single
        /// element matching the <paramref name="key"/> or <see langword="default"/> ( <typeparamref
        /// name="T"/> ) if no such element is found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// More than one element matches <paramref name="key"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> GetAsync<T>(this IRepository<T> repository, object key, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
                await r.GetAsync(ctx, key, cancellationToken), cancellationToken);

        private static async Task<T?> GetAsync<T>(this IInternalRepository<T> repository, DbContext context, object key, CancellationToken cancellationToken = default) where T : class
            => await new ContextQueryable<T>(repository.EntityQuery(context), context).FindAsync(key, cancellationToken);

        #endregion Get

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
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                ctx.Remove(entity);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

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
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
            {
                var entity = await r.GetAsync(ctx, key, cancellationToken);
                if (entity == null) return false;
                ctx.Remove(entity);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

        /// <summary>
        /// Delete multiple entities from a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> to delete <paramref name="entities"/> from.
        /// </param>
        /// <param name="entities">The entities to delete.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if any entities were deleted, <see langword="false"/> if no entities
        /// were deleted.
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
        public static async Task<bool> DeleteRangeAsync<T>(this IRepository<T> repository, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                ctx.RemoveRange(entities);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

        /// <summary>
        /// Delete multiple entities from a repository based upon the specified keys.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="keys"/> must contain objects with properties that match the primary key
        /// structure of <typeparamref name="T"/>.
        /// </para>
        /// <para>For an entity whose primary key is a single column (Id). Use new { Id = ? }.</para>
        /// <para>
        /// For an entity whose primary key has multiple columns (Id1, Id2,...). Use new { Id1 = ?,
        /// Id2 = ?,... }.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to delete entities from.</param>
        /// <param name="keys">
        /// An <see cref="IEnumerable{T}"/> containing objects that specify all primary key fields.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if any entities were deleted, <see langword="false"/> if no entities
        /// were deleted.
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
        public static async Task<bool> DeleteRangeByIdAsync<T>(this IRepository<T> repository, IEnumerable<object> keys, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
            {
                await Task.WhenAll(keys.Select(async key =>
                {
                    var entity = await r.GetAsync(ctx, key, cancellationToken);
                    if (entity != null)
                        ctx.Remove(entity);
                }));
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

        #endregion Delete

        #region Update

        /// <summary>
        /// Update an entity from a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to update the <param name="entity"/>.</param>
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
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
            {
                var current = await r.GetAsync(ctx, entity, cancellationToken);
                if (current == null) return default;
                ctx.Entry(current).CurrentValues.SetValues(entity);
                await r.HandleExpressionUpdateAsync(ctx, current, entity, cancellationToken);
                var res = await ctx.SaveChangesAsync(cancellationToken);
                return res > 0 ? current : default;
            }, cancellationToken);

        /// <summary>
        /// Updates entities from a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to update the <param name="entities"/>.</param>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an array of
        /// the updated entities.
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
        public static async Task<T?[]?> UpdateRangeAsync<T>(this IRepository<T> repository, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
            {
                var updates = await Task.WhenAll(entities.Select(async entity =>
                {
                    var current = await r.GetAsync(ctx, entity, cancellationToken);
                    if (current == null) return default;
                    ctx.Entry(current).CurrentValues.SetValues(entity);
                    await r.HandleExpressionUpdateAsync(ctx, current, entity, cancellationToken);
                    return current;
                }));
                var res = await ctx.SaveChangesAsync(cancellationToken);
                return res > 0 ? updates : default;
            }, cancellationToken);

        #endregion Update

        #region Insert

        /// <summary>
        /// Insert an entity into a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to insert the <param name="entity"/>.</param>
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
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                var e = ctx.Attach(entity);
                var res = await ctx.SaveChangesAsync(cancellationToken);
                return res > 0 ? e.Entity : default;
            }, cancellationToken);

        /// <summary>
        /// Insert entities into a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IRepository{T}"/> to insert the <param name="entities"/>.</param>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an array of
        /// the inserted entities.
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
        public static async Task<T?[]?> InsertRangeAsync<T>(this IRepository<T> repository, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                var updates = await Task.WhenAll(entities.Select(entity =>
                Task.Run(() =>
                {
                    var e = ctx.Attach(entity);
                    return e.Entity;
                })));
                var res = await ctx.SaveChangesAsync(cancellationToken);
                return res > 0 ? updates : default;
            }, cancellationToken);

        #endregion Insert

        #region Execute

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalRepository<T> repository, Func<IInternalRepository<T>, DbContext, Task<TResult>> expression, CancellationToken cancellationToken = default)
            where T : class
        {
            using var ctx = await repository.Factory.CreateDbContextAsync(cancellationToken);
            return await expression(repository, ctx);
        }

        #endregion Execute
    }
}