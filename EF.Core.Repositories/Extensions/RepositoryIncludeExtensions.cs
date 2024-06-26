﻿using EF.Core.Repositories.Internal.Base;
using EF.Core.Repositories.Internal.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Include extension methods for <see cref="IRepository{T}"/> and <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryIncludeExtensions
    {
        /// <summary>
        /// Specifies related entities to include in the <see cref="IRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity being
        /// queried. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="RepositoryThenIncludeExtensions.ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeRepository<TEntity, TProp> Include<TEntity, TProp>(this IRepository<TEntity> repository, Expression<Func<TEntity, ICollection<TProp>>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeCollectionRepository<TEntity, TProp>(repository, expression);
        }

        /// <summary>
        /// Specifies related entities to include in the <see cref="IReadOnlyRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity being
        /// queried. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="RepositoryThenIncludeExtensions.ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeReadOnlyRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IReadOnlyRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeReadOnlyRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeReadOnlyRepository<TEntity, TProp> Include<TEntity, TProp>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, ICollection<TProp>>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeCollectionReadOnlyRepository<TEntity, TProp>(repository, expression);
        }

        /// <summary>
        /// Specifies related entities to include in the <see cref="IRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity being
        /// queried. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="RepositoryThenIncludeExtensions.ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeRepository<TEntity, TProp> Include<TEntity, TProp>(this IRepository<TEntity> repository, Expression<Func<TEntity, TProp>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeRepository<TEntity, TProp>(repository, expression);
        }

        /// <summary>
        /// Specifies related entities to include in the <see cref="IReadOnlyRepository{T}"/>. The
        /// navigation property to be included is specified starting with the type of entity being
        /// queried. If you wish to include additional types based on the navigation properties of
        /// the type being included, then chain a call to <see
        /// cref="RepositoryThenIncludeExtensions.ThenInclude{TEntity, TPrevProp,
        /// TProp}(IIncludeReadOnlyRepository{TEntity, TPrevProp}, Expression{Func{TPrevProp,
        /// ICollection{TProp}}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProp">The type of the related entity to be included.</typeparam>
        /// <param name="repository">The source <see cref="IReadOnlyRepository{T}"/></param>
        /// <param name="expression">
        /// A lambda expression representing the navigation property to be included (t =&gt; t.Property1).
        /// </param>
        /// <returns>
        /// An <see cref="IIncludeReadOnlyRepository{TEntity, TProp}"/> with the related data included.
        /// </returns>
        public static IIncludeReadOnlyRepository<TEntity, TProp> Include<TEntity, TProp>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TProp>> expression)
            where TEntity : class
            where TProp : class?
        {
            return new IncludeReadOnlyRepository<TEntity, TProp>(repository, expression);
        }

        private sealed class IncludeCollectionReadOnlyRepository<TEntity, TProp> : IncludeReadOnlyRepositoryBase<TEntity, TProp>, IExtensibleIncludeCollectionRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            private readonly Expression<Func<TEntity, ICollection<TProp>>> _expression;

            public IncludeCollectionReadOnlyRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, ICollection<TProp>>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, ICollection<TProp>> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Include(_expression);
            }
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

        private sealed class IncludeReadOnlyRepository<TEntity, TProp> : IncludeReadOnlyRepositoryBase<TEntity, TProp>, IExtensibleIncludeRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            private readonly Expression<Func<TEntity, TProp>> _expression;

            public IncludeReadOnlyRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TProp>> expression) : base(source)
            {
                _expression = expression;
            }

            public override IIncludableQueryable<TEntity, TProp> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Include(_expression);
            }
        }

        private abstract class IncludeReadOnlyRepositoryBase<TEntity, TProp> : WrapperReadOnlyRepositoryBase<TEntity, IInternalReadOnlyRepository<TEntity>>, IInternalIncludeReadOnlyRepository<TEntity, TProp>
            where TEntity : class
            where TProp : class?
        {
            protected IncludeReadOnlyRepositoryBase(IReadOnlyRepository<TEntity> source) : base((IInternalReadOnlyRepository<TEntity>)source)
            { }
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