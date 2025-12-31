using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Set extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositorySetExtensions
    {
        /// <summary>
        /// Produces the set intersection of two repositories by using the default equality comparer
        /// to compare values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source2"/> are returned.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source1"/> are returned.
        /// </param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set intersection of the two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Intersect<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new IntersectRepository<T>(source1, source2);
        }

        /// <summary>
        /// Produces the set intersection of two repositories by using the specified <see
        /// cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source2"/> are returned.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source1"/> are returned.
        /// </param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set intersection of the two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Intersect<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new IntersectRepository<T>(source1, source2, comparer);
        }

        /// <summary>
        /// Produces the set intersection of two repositories by using the specified key selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source2"/> are returned.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source1"/> are returned.
        /// </param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set intersection of the two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> IntersectBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<TKey> source2,
            Expression<Func<T, TKey>> keySelector)
        {
            return new IntersectByRepository<T, TKey>(source1, source2, keySelector);
        }

        /// <summary>
        /// Produces the set intersection of two repositories by using the specified key selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source2"/> are returned.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements that also appear in
        /// <paramref name="source1"/> are returned.
        /// </param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set intersection of the two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> IntersectBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<TKey> source2,
            Expression<Func<T, TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new IntersectByRepository<T, TKey>(source1, source2, keySelector, comparer);
        }

        /// <summary>
        /// Produces the set union of two repositories by using the default equality comparer to
        /// compare values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the first set for
        /// the union operation.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the second set for
        /// the union operation.
        /// </param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the elements from both
        /// repositories, excluding duplicates.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Union<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new UnionRepository<T>(source1, source2);
        }

        /// <summary>
        /// Produces the set union of two repositories by using the specified <see
        /// cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the first set for
        /// the union operation.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the second set for
        /// the union operation.
        /// </param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the elements from both
        /// repositories, excluding duplicates.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Union<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new UnionRepository<T>(source1, source2, comparer);
        }

        /// <summary>
        /// Produces the set union of two repositories according to a specified key selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the first set for
        /// the union operation.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the second set for
        /// the union operation.
        /// </param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the elements from both
        /// repositories, excluding duplicates.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> UnionBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            Expression<Func<T, TKey>> keySelector)
        {
            return new UnionByRepository<T, TKey>(source1, source2, keySelector);
        }

        /// <summary>
        /// Produces the set union of two repositories according to a specified key selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the repositories.</typeparam>
        /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
        /// <param name="source1">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the first set for
        /// the union operation.
        /// </param>
        /// <param name="source2">
        /// A <see cref="IReadOnlyRepository{T}"/> whose distinct elements form the second set for
        /// the union operation.
        /// </param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the elements from both
        /// repositories, excluding duplicates.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> UnionBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            Expression<Func<T, TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new UnionByRepository<T, TKey>(source1, source2, keySelector, comparer);
        }

        private sealed class IntersectByRepository<T, TKey>(IReadOnlyRepository<T> source1, IReadOnlyRepository<TKey> source2, Expression<Func<T, TKey>> keySelector, IEqualityComparer<TKey>? comparer = null)
            : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source1)
        {
            private readonly IEqualityComparer<TKey>? _comparer = comparer;
            private readonly IInternalReadOnlyRepository<TKey> _internalSource2 = (IInternalReadOnlyRepository<TKey>)source2;
            private readonly Expression<Func<T, TKey>> _keySelector = keySelector;

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).IntersectBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector);
                else
                    return _internalSource.EntityQuery(context).IntersectBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector,
                        _comparer);
            }
        }

        private sealed class IntersectRepository<T>(IReadOnlyRepository<T> source1, IReadOnlyRepository<T> source2, IEqualityComparer<T>? comparer = null)
            : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source1)
        {
            private readonly IEqualityComparer<T>? _comparer = comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2 = (IInternalReadOnlyRepository<T>)source2;

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Intersect(
                        _internalSource2.EntityQuery(context));
                else
                    return _internalSource.EntityQuery(context).Intersect(
                        _internalSource2.EntityQuery(context),
                        _comparer);
            }
        }

        private sealed class UnionByRepository<T, TKey>(IReadOnlyRepository<T> source1, IReadOnlyRepository<T> source2, Expression<Func<T, TKey>> keySelector, IEqualityComparer<TKey>? comparer = null)
            : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source1)
        {
            private readonly IEqualityComparer<TKey>? _comparer = comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
            private readonly Expression<Func<T, TKey>> _keySelector = keySelector;

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).UnionBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector);
                else
                    return _internalSource.EntityQuery(context).UnionBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector,
                        _comparer);
            }
        }

        private sealed class UnionRepository<T>(IReadOnlyRepository<T> source1, IReadOnlyRepository<T> source2, IEqualityComparer<T>? comparer = null)
            : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source1)
        {
            private readonly IEqualityComparer<T>? _comparer = comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2 = (IInternalReadOnlyRepository<T>)source2;

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Union(
                        _internalSource2.EntityQuery(context));
                else
                    return _internalSource.EntityQuery(context).Union(
                        _internalSource2.EntityQuery(context),
                        _comparer);
            }
        }
    }
}