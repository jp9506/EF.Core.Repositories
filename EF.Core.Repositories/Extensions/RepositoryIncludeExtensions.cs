using EF.Core.Repositories.Internal.Base;
using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryIncludeExtensions
    {
        public static IIncludeRepository<TEntity, TProp> Include<TEntity, TProp>(this IRepository<TEntity> repository, Expression<Func<TEntity, ICollection<TProp>>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeCollectionRepository<TEntity, TProp>(repository, expression);
        }

        public static IIncludeRepository<TEntity, TProp> Include<TEntity, TProp>(this IRepository<TEntity> repository, Expression<Func<TEntity, TProp>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeRepository<TEntity, TProp>(repository, expression);
        }

        private sealed class IncludeCollectionRepository<TEntity, TProp> : IncludeRepositoryBase<TEntity, TProp>, IExtensibleIncludeCollectionRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            private readonly Expression<Func<TEntity, ICollection<TProp>>> _expression;

            public IncludeCollectionRepository(IRepository<TEntity> source, Expression<Func<TEntity, ICollection<TProp>>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, ICollection<TProp>> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Include(_expression);
            }

            public override async Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default)
            {
                await _internalSource.HandleExpressionUpdateAsync(context, current, entity, cancellationToken);

                var exp = _expression.Compile();
                var curProp = exp(current);
                var newProp = exp(entity);
                if (curProp != null && newProp != null)
                {
                    var toAdd = newProp.Where(x => x != null && !curProp.Any(y => y != null && context.GetKeyHashCode(y) == context.GetKeyHashCode(x))).ToArray();
                    var toDelete = curProp.Where(x => x != null && !newProp.Any(y => y != null && context.GetKeyHashCode(y) == context.GetKeyHashCode(x))).ToArray();
                    var intersection = curProp.Where(x => x != null).Join(newProp, x => context.GetKeyHashCode(x), x => context.GetKeyHashCode(x), (c, n) => (c, n)).ToArray();
                    await Task.WhenAll(toAdd.Select(x => Task.Run(() => curProp.Add(x), cancellationToken)));
                    await Task.WhenAll(toDelete.Select(x => Task.Run(() => curProp.Remove(x), cancellationToken)));
                    await Task.WhenAll(intersection.Select(async x => await then(x.c, x.n)));
                }
            }
        }

        private sealed class IncludeRepository<TEntity, TProp> : IncludeRepositoryBase<TEntity, TProp>, IExtensibleIncludeRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            private readonly Expression<Func<TEntity, TProp>> _expression;

            public IncludeRepository(IRepository<TEntity> source, Expression<Func<TEntity, TProp>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, TProp> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Include(_expression);
            }

            public override async Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default)
            {
                await _internalSource.HandleExpressionUpdateAsync(context, current, entity, cancellationToken);

                var exp = _expression.Compile();
                var curProp = exp(current);
                var newProp = exp(entity);
                if (curProp != null && newProp != null)
                {
                    await then(curProp, newProp);
                }
            }
        }

        private abstract class IncludeRepositoryBase<TEntity, TProp> : WrapperRepositoryBase<TEntity, IInternalRepository<TEntity>>, IInternalIncludeRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            protected IncludeRepositoryBase(IRepository<TEntity> source) : base((IInternalRepository<TEntity>)source)
            {
            }

            public abstract Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default);

            public override async Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, CancellationToken cancellationToken = default)
            {
                await HandleExpressionUpdateAsync(context, current, entity, async (curE, newE) => await Task.Run(() =>
                {
                    if (curE != null && newE != null)
                    {
#pragma warning disable CS8634
                        context.Entry(curE).CurrentValues.SetValues(newE);
#pragma warning restore CS8634
                    }
                }, cancellationToken), cancellationToken);
            }
        }
    }
}