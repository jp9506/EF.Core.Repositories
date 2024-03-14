using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryOrderByExtensions
    {
        /// <summary>
        /// Performs ordering of the elements in a repository in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> OrderBy<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, true);
        }

        /// <summary>
        /// Performs ordering of the elements in a repository in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> that contains elements to sort.
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
        public static IOrderedRepository<T> OrderBy<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, comparer, true);
        }

        /// <summary>
        /// Performs ordering of the elements in a repository in descending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> that contains elements to sort.
        /// </param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        /// An <see cref="IOrderedRepository{T}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedRepository<T> OrderByDescending<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, false);
        }

        /// <summary>
        /// Performs ordering of the elements in a repository in descending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function represented by <paramref name="keySelector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IRepository{T}"/> that contains elements to sort.
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
        public static IOrderedRepository<T> OrderByDescending<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, comparer, false);
        }

        private sealed class OrderedRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>, IInternalOrderedRepository<T>
        {
            private readonly bool _ascending;
            private readonly IComparer<TKey>? _comparer;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public OrderedRepository(IReadOnlyRepository<T> source, Expression<Func<T, TKey>> keySelector, bool ascending) : base((IInternalReadOnlyRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = null;
            }

            public OrderedRepository(IReadOnlyRepository<T> source, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer, bool ascending) : base((IInternalReadOnlyRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = comparer;
            }

            public IOrderedQueryable<T> EntityOrdered(DbContext context)
            {
                if (_comparer == null)
                    return _ascending
                        ? _internalSource.EntityQuery(context).OrderBy(_keySelector)
                        : _internalSource.EntityQuery(context).OrderByDescending(_keySelector);
                else
                    return _ascending
                        ? _internalSource.EntityQuery(context).OrderBy(_keySelector, _comparer)
                        : _internalSource.EntityQuery(context).OrderByDescending(_keySelector, _comparer);
            }

            public override IQueryable<T> EntityQuery(DbContext context) => EntityOrdered(context);
        }
    }
}