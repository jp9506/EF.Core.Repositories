using EF.Core.Repositories.Internal;
using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Extensions
{
    public static class IRepositoryExtensions
    {
        #region Get

        public static async Task<T?> GetAsync<T>(this IRepository<T> repository, object key, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
                await r.GetAsync(ctx, key, cancellationToken), cancellationToken);

        private static async Task<T?> GetAsync<T>(this IInternalRepository<T> repository, DbContext context, object key, CancellationToken cancellationToken = default) where T : class
            => await new ContextQueryable<T>(repository.EntityQuery(context), context).FindAsync(key, cancellationToken);

        #endregion Get

        #region Delete

        public static async Task<bool> DeleteAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                ctx.Remove(entity);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

        public static async Task<bool> DeleteByIdAsync<T>(this IRepository<T> repository, object key, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (r, ctx) =>
            {
                var entity = await r.GetAsync(ctx, key, cancellationToken);
                if (entity == null) return false;
                ctx.Remove(entity);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

        public static async Task<bool> DeleteRangeAsync<T>(this IRepository<T> repository, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                ctx.RemoveRange(entities);
                return (await ctx.SaveChangesAsync(cancellationToken)) > 0;
            }, cancellationToken);

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

        public static async Task<T?> InsertAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default) where T : class
            => await ((IInternalRepository<T>)repository).ExecuteAsync(async (_, ctx) =>
            {
                var e = ctx.Attach(entity);
                var res = await ctx.SaveChangesAsync(cancellationToken);
                return res > 0 ? e.Entity : default;
            }, cancellationToken);

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