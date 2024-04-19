using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryWhereExtensions
    {
        /// <summary>
        /// Filters a repository of values based on a predicate.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from <paramref
        /// name="repository"/> that satisfy the condition specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Where<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate)
        {
            return new WhereRepository<T>(repository, predicate);
        }

        /// <summary>
        /// Filters a repository of values based on a predicate. Each element's index is used in the
        /// logic of the predicate function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/> to filter.</param>
        /// <param name="predicate">
        /// A function to test each element for a condition; the second parameter of the function
        /// represents the index of the element in the <paramref name="repository"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from <paramref
        /// name="repository"/> that satisfy the condition specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> Where<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int, bool>> predicate)
        {
            return new WhereRepository<T>(repository, predicate);
        }

        private sealed class WhereRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly Expression<Func<T, bool>>? _predicate;
            private readonly Expression<Func<T, int, bool>>? _predicate2;

            public WhereRepository(IReadOnlyRepository<T> source, Expression<Func<T, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = predicate;
                _predicate2 = null;
            }

            public WhereRepository(IReadOnlyRepository<T> source, Expression<Func<T, int, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = null;
                _predicate2 = predicate;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_predicate != null)
                    return _internalSource.EntityQuery(context).Where(_predicate);
                if (_predicate2 != null)
                    return _internalSource.EntityQuery(context).Where(_predicate2);
                return Enumerable.Empty<T>().AsQueryable();
            }
        }
    }
}