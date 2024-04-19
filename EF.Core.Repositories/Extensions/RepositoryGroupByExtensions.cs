using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Group By extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryGroupByExtensions
    {
        /// <summary>
        /// Groups the elements of a repository according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <see
        /// cref="IGrouping{TKey, TElement}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<IGrouping<TKey, TEntity>> GroupBy<TEntity, TKey>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector)
        {
            return new GroupByRepository<TEntity, TKey>(repository, keySelector);
        }

        /// <summary>
        /// Groups the elements of a repository according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <see
        /// cref="IGrouping{TKey, TElement}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> or <paramref
        /// name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<IGrouping<TKey, TEntity>> GroupBy<TEntity, TKey>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return new GroupByRepository<TEntity, TKey>(repository, keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements of a repository according to a specified key selector function and
        /// projects the elements for each group using the specified function.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the elements in each <see cref="IGrouping{TKey, TElement}"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">
        /// A function to map each source element to an element in an <see cref="IGrouping{TKey, TElement}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <see
        /// cref="IGrouping{TKey, TElement}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> or <paramref
        /// name="elementSelector"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<IGrouping<TKey, TElement>> GroupBy<TEntity, TKey, TElement>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector)
        {
            return new GroupByRepository<TEntity, TKey, TElement>(repository, keySelector, elementSelector);
        }

        /// <summary>
        /// Groups the elements of a repository according to a specified key selector function and
        /// projects the elements for each group using the specified function.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <typeparam name="TElement">
        /// The type of the elements in each <see cref="IGrouping{TKey, TElement}"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to group.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">
        /// A function to map each source element to an element in an <see cref="IGrouping{TKey, TElement}"/>.
        /// </param>
        /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <see
        /// cref="IGrouping{TKey, TElement}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> or <paramref
        /// name="elementSelector"/> or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<IGrouping<TKey, TElement>> GroupBy<TEntity, TKey, TElement>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            return new GroupByRepository<TEntity, TKey, TElement>(repository, keySelector, elementSelector, comparer);
        }

        private sealed class GroupByRepository<TEntity, TKey> : WrapperReadOnlyRepositoryBase<TEntity, IInternalReadOnlyRepository<TEntity>, IGrouping<TKey, TEntity>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Expression<Func<TEntity, TKey>> _keySelector;

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = null;
                _keySelector = keySelector;
            }

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = comparer;
                _keySelector = keySelector;
            }

            public override IQueryable<IGrouping<TKey, TEntity>> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector);
                else
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _comparer);
            }
        }

        private sealed class GroupByRepository<TEntity, TKey, TElement> : WrapperReadOnlyRepositoryBase<TEntity, IInternalReadOnlyRepository<TEntity>, IGrouping<TKey, TElement>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Expression<Func<TEntity, TElement>> _elementSelector;
            private readonly Expression<Func<TEntity, TKey>> _keySelector;

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = null;
                _elementSelector = elementSelector;
                _keySelector = keySelector;
            }

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector, IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = comparer;
                _elementSelector = elementSelector;
                _keySelector = keySelector;
            }

            public override IQueryable<IGrouping<TKey, TElement>> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _elementSelector);
                else
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _elementSelector, _comparer);
            }
        }
    }
}