using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Take extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryTakeExtensions
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the first <paramref name="count"/>
        /// of elements from the <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/></exception>
        public static IReadOnlyRepository<T> Take<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new TakeRepository<T>(repository, count, false);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the last <paramref name="count"/>
        /// elements from <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/></exception>
        public static IReadOnlyRepository<T> TakeLast<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new TakeRepository<T>(repository, count, true);
        }

        /// <summary>
        /// Returns elements from a repository as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from the <paramref
        /// name="repository"/> occurring before the element at which the test specified by
        /// <paramref name="predicate"/> no longer passes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> TakeWhile<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate)
        {
            return new TakeRepository<T>(repository, predicate);
        }

        /// <summary>
        /// Returns elements from a repository as long as a specified condition is true. The
        /// element's index is used in the logic of the <paramref name="predicate"/> function.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition; the second parameter of the function
        /// represents the index of the element in the <paramref name="repository"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains elements from the <paramref
        /// name="repository"/> occurring before the element at which the test specified by
        /// <paramref name="predicate"/> no longer passes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<T> TakeWhile<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int, bool>> predicate)
        {
            return new TakeRepository<T>(repository, predicate);
        }

        private sealed class TakeRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly int _count;
            private readonly bool _last;
            private readonly Expression<Func<T, bool>>? _predicate;
            private readonly Expression<Func<T, int, bool>>? _predicate2;

            public TakeRepository(IReadOnlyRepository<T> source, int count, bool last) : base((IInternalReadOnlyRepository<T>)source)
            {
                _count = count;
                _last = last;
                _predicate = null;
                _predicate2 = null;
            }

            public TakeRepository(IReadOnlyRepository<T> source, Expression<Func<T, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = predicate;
                _predicate2 = null;
            }

            public TakeRepository(IReadOnlyRepository<T> source, Expression<Func<T, int, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = null;
                _predicate2 = predicate;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_predicate != null)
                    return _internalSource.EntityQuery(context).TakeWhile(_predicate);
                if (_predicate2 != null)
                    return _internalSource.EntityQuery(context).TakeWhile(_predicate2);
                return _last ?
                    _internalSource.EntityQuery(context).TakeLast(_count) :
                    _internalSource.EntityQuery(context).Take(_count);
            }
        }
    }
}