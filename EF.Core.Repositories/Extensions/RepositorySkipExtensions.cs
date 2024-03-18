using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySkipExtensions
    {
        /// <summary>
        /// Bypasses a specified number of elements in a repository and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements that occur after the
        /// specified index in the <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/></exception>
        public static IReadOnlyRepository<T> Skip<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new SkipRepository<T>(repository, count, false);
        }

        /// <summary>
        /// Bypasses a specified number of elements from the end of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="count">The number of elements to omit from the end of the <paramref name="repository"/>.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements from source minus
        /// count elements from the end of the <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/></exception>
        public static IReadOnlyRepository<T> SkipLast<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new SkipRepository<T>(repository, count, true);
        }

        /// <summary>
        /// Bypasses elements in a repository as long as a specified condition is true and then
        /// returns the remaining elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from the <paramref
        /// name="repository"/> starting at the first element in the <paramref name="repository"/>
        /// that does not pass the test specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> SkipWhile<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate)
        {
            return new SkipRepository<T>(repository, predicate);
        }

        /// <summary>
        /// Bypasses elements in a repository as long as a specified condition is true and then
        /// returns the remaining elements. The element's index is used in the logic of the
        /// <paramref name="predicate"/> function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition; the second parameter of this function
        /// represents the index of the element in the <paramref name="repository"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from the <paramref
        /// name="repository"/> starting at the first element in the <paramref name="repository"/>
        /// that does not pass the test specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> SkipWhile<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int, bool>> predicate)
        {
            return new SkipRepository<T>(repository, predicate);
        }

        private sealed class SkipRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly int _count;
            private readonly bool _last;
            private readonly Expression<Func<T, bool>>? _predicate;
            private readonly Expression<Func<T, int, bool>>? _predicate2;

            public SkipRepository(IReadOnlyRepository<T> source, int count, bool last) : base((IInternalReadOnlyRepository<T>)source)
            {
                _count = count;
                _last = last;
                _predicate = null;
            }

            public SkipRepository(IReadOnlyRepository<T> source, Expression<Func<T, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = predicate;
                _predicate2 = null;
            }

            public SkipRepository(IReadOnlyRepository<T> source, Expression<Func<T, int, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = null;
                _predicate2 = predicate;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_predicate != null)
                    return _internalSource.EntityQuery(context).SkipWhile(_predicate);
                if (_predicate2 != null)
                    return _internalSource.EntityQuery(context).SkipWhile(_predicate2);
                return _last ?
                    _internalSource.EntityQuery(context).SkipLast(_count) :
                    _internalSource.EntityQuery(context).Skip(_count);
            }
        }
    }
}