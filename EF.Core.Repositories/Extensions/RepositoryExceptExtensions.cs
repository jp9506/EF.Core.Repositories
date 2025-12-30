using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Except extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryExceptExtensions
    {
        /// <summary>
        /// Produces the set difference of two repositories by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source1"/>.</typeparam>
        /// <param name="source1">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements that are not also in <paramref
        /// name="source2"/> will be returned.
        /// </param>
        /// <param name="source2">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements that also occur in <paramref
        /// name="source1"/> will not appear in the returned repository.
        /// </param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set difference of the elements
        /// of two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Except<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new ExceptRepository<T>(source1, source2);
        }

        /// <summary>
        /// Produces the set difference of two repositories by using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source1"/>.</typeparam>
        /// <param name="source1">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements that are not also in <paramref
        /// name="source2"/> will be returned.
        /// </param>
        /// <param name="source2">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements that also occur in <paramref
        /// name="source1"/> will not appear in the returned repository.
        /// </param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>
        /// A <see cref="IReadOnlyRepository{T}"/> that contains the set difference of the elements
        /// of two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> or <paramref name="comparer"/>
        /// is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Except<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new ExceptRepository<T>(source1, source2, comparer);
        }

        private sealed class ExceptRepository<T>(IReadOnlyRepository<T> source1, IReadOnlyRepository<T> source2, IEqualityComparer<T>? comparer = null)
            : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source1)
        {
            private readonly IEqualityComparer<T>? _comparer = comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2 = (IInternalReadOnlyRepository<T>)source2;

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Except(_internalSource2.EntityQuery(context));
                else
                    return _internalSource.EntityQuery(context).Except(_internalSource2.EntityQuery(context), _comparer);
            }
        }
    }
}