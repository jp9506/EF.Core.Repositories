using EF.Core.Repositories.Internal.Base;
using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryThenIncludeExtensions
    {
        /// <summary>
        /// Specifies additional related entities to include in the <see cref="IRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity previously
        /// included. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPrevProp">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeRepository<TEntity, TProp> ThenInclude<TEntity, TPrevProp, TProp>(this IIncludeRepository<TEntity, TPrevProp> repository, Expression<Func<TPrevProp, ICollection<TProp>>> expression)
            where TEntity : class
            where TPrevProp : class?
            where TProp : class?
        {
            return new ThenIncludeCollectionRepository<TEntity, TPrevProp, TProp>(repository, expression);
        }

        /// <summary>
        /// Specifies additional related entities to include in the <see cref="IRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity previously
        /// included. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TPrevProp">The type of the entity that was just included.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeRepository<TEntity, TProp> ThenInclude<TEntity, TPrevProp, TProp>(this IIncludeRepository<TEntity, TPrevProp> repository, Expression<Func<TPrevProp, TProp>> expression)
            where TEntity : class
            where TPrevProp : class?
            where TProp : class?
        {
            return new ThenIncludeRepository<TEntity, TPrevProp, TProp>(repository, expression);
        }

        private sealed class ThenIncludeCollectionRepository<TEntity, TPrevProp, TProp> : ThenIncludeRepositoryBase<TEntity, TPrevProp, TProp>, IExtensibleIncludeCollectionRepository<TEntity, TProp>
            where TEntity : class
            where TPrevProp : class?
            where TProp : class?
        {
            private readonly Expression<Func<TPrevProp, ICollection<TProp>>> _expression;

            public ThenIncludeCollectionRepository(IIncludeRepository<TEntity, TPrevProp> source, Expression<Func<TPrevProp, ICollection<TProp>>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, ICollection<TProp>> EntityQuery(DbContext context)
            {
                if (_internalSource is IExtensibleIncludeRepository<TEntity, TPrevProp> source)
                {
                    return source.EntityQuery(context).ThenInclude(_expression);
                }
                if (_internalSource is IExtensibleIncludeCollectionRepository<TEntity, TPrevProp> colSource)
                {
                    return colSource.EntityQuery(context).ThenInclude(_expression);
                }
                throw new NotImplementedException();
            }

            public override async Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default)
            {
                var exp = _expression.Compile();
                await _internalSource.HandleExpressionUpdateAsync(context, current, entity,
                    async (curE, newE) =>
                    {
                        var curProp = exp(curE);
                        var newProp = exp(newE);
                        if (curProp != null && newProp != null)
                        {
                            var toAdd = newProp.Where(x => x != null && !curProp.Any(y => y != null && context.GetKeyHashCode(y) == context.GetKeyHashCode(x))).ToArray();
                            var toDelete = curProp.Where(x => x != null && !newProp.Any(y => y != null && context.GetKeyHashCode(y) == context.GetKeyHashCode(x))).ToArray();
                            var intersection = curProp.Where(x => x != null).Join(newProp, x => context.GetKeyHashCode(x), x => context.GetKeyHashCode(x), (c, n) => (c, n)).ToArray();
                            await Task.WhenAll(toAdd.Select(x => Task.Run(() => curProp.Add(x), cancellationToken)));
                            await Task.WhenAll(toDelete.Select(x => Task.Run(() => curProp.Remove(x), cancellationToken)));
                            await Task.WhenAll(intersection.Select(async x => await then(x.c, x.n)));
                        }
                    }, cancellationToken);
            }
        }

        private sealed class ThenIncludeRepository<TEntity, TPrevProp, TProp> : ThenIncludeRepositoryBase<TEntity, TPrevProp, TProp>, IExtensibleIncludeRepository<TEntity, TProp>
            where TEntity : class
            where TPrevProp : class?
            where TProp : class?
        {
            private readonly Expression<Func<TPrevProp, TProp>> _expression;

            public ThenIncludeRepository(IIncludeRepository<TEntity, TPrevProp> source, Expression<Func<TPrevProp, TProp>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, TProp> EntityQuery(DbContext context)
            {
                if (_internalSource is IExtensibleIncludeRepository<TEntity, TPrevProp> source)
                {
                    return source.EntityQuery(context).ThenInclude(_expression);
                }
                if (_internalSource is IExtensibleIncludeCollectionRepository<TEntity, TPrevProp> colSource)
                {
                    return colSource.EntityQuery(context).ThenInclude(_expression);
                }
                throw new NotImplementedException();
            }

            public override async Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default)
            {
                var exp = _expression.Compile();
                await _internalSource.HandleExpressionUpdateAsync(context, current, entity,
                    async (curE, newE) =>
                    {
                        var curProp = exp(curE);
                        var newProp = exp(newE);
                        if (curProp != null && newProp != null)
                        {
                            await then(curProp, newProp);
                        }
                    }, cancellationToken);
            }
        }

        private abstract class ThenIncludeRepositoryBase<TEntity, TPrevProp, TProp> : WrapperRepositoryBase<TEntity, IInternalIncludeRepository<TEntity, TPrevProp>>, IInternalIncludeRepository<TEntity, TProp>
            where TEntity : class
            where TPrevProp : class?
            where TProp : class?
        {
            protected ThenIncludeRepositoryBase(IIncludeRepository<TEntity, TPrevProp> source) : base((IInternalIncludeRepository<TEntity, TPrevProp>)source)
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

#pragma warning restore S2436 // Types and methods should not have too many generic parameters