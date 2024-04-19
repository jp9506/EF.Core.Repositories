using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryThenByExtensions
    {
        /// <summary>
        /// Performs a subsequent ordering of the elements in a repository in ascending order
        /// according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IOrderedRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> ThenBy<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, true);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a repository in ascending order
        /// according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IOrderedRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="IComparer{TKey}"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> or <paramref
        /// name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> ThenBy<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, comparer, true);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a repository in descending order
        /// according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IOrderedRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> ThenByDescending<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, false);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a repository in descending order
        /// according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IOrderedRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An <see cref="IComparer{TKey}"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> or <paramref
        /// name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> ThenByDescending<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, comparer, false);
        }

        private sealed class ThenOrderedRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalOrderedRepository<T>>, IInternalOrderedRepository<T>
        {
            private readonly bool _ascending;
            private readonly IComparer<TKey>? _comparer;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public ThenOrderedRepository(IOrderedRepository<T> source, Expression<Func<T, TKey>> keySelector, bool ascending) : base((IInternalOrderedRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = null;
            }

            public ThenOrderedRepository(IOrderedRepository<T> source, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer, bool ascending) : base((IInternalOrderedRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = comparer;
            }

            public IOrderedQueryable<T> EntityOrdered(DbContext context)
            {
                if (_comparer == null)
                    return _ascending
                        ? _internalSource.EntityOrdered(context).ThenBy(_keySelector)
                        : _internalSource.EntityOrdered(context).ThenByDescending(_keySelector);
                else
                    return _ascending
                        ? _internalSource.EntityOrdered(context).ThenBy(_keySelector, _comparer)
                        : _internalSource.EntityOrdered(context).ThenByDescending(_keySelector, _comparer);
            }

            public override IQueryable<T> EntityQuery(DbContext context) => EntityOrdered(context);
        }
    }
}