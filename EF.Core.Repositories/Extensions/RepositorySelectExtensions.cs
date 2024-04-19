using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySelectExtensions
    {
        /// <summary>
        /// Projects each element of a repository into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the value returned by the function represented by selector.
        /// </typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/> to project.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements are the result of invoking a
        /// projection function on each element of <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<TResult> Select<TSource, TResult>(this IReadOnlyRepository<TSource> repository, Expression<Func<TSource, TResult>> selector)
        {
            return new SelectRepository<TSource, TResult>(repository, selector);
        }

        /// <summary>
        /// Projects each element of a repository into a new form. Each element's index is used in
        /// the logic of the projection function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the value returned by the function represented by selector.
        /// </typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/> to project.</param>
        /// <param name="selector">
        /// A projection function to apply to each element; the second parameter of the function
        /// represents the index of the element in the <paramref name="repository"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements are the result of invoking a
        /// projection function on each element of <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<TResult> Select<TSource, TResult>(this IReadOnlyRepository<TSource> repository, Expression<Func<TSource, int, TResult>> selector)
        {
            return new SelectRepository<TSource, TResult>(repository, selector);
        }

        private sealed class SelectRepository<TSource, TResult> : WrapperReadOnlyRepositoryBase<TSource, IInternalReadOnlyRepository<TSource>, TResult>
        {
            private readonly Expression<Func<TSource, TResult>>? _selector;
            private readonly Expression<Func<TSource, int, TResult>>? _selector2;

            public SelectRepository(IReadOnlyRepository<TSource> source, Expression<Func<TSource, TResult>> selector) : base((IInternalReadOnlyRepository<TSource>)source)
            {
                _selector = selector;
                _selector2 = null;
            }

            public SelectRepository(IReadOnlyRepository<TSource> source, Expression<Func<TSource, int, TResult>> selector) : base((IInternalReadOnlyRepository<TSource>)source)
            {
                _selector = null;
                _selector2 = selector;
            }

            public override IQueryable<TResult> EntityQuery(DbContext context)
            {
                if (_selector != null)
                    return _internalSource.EntityQuery(context).Select(_selector);
                if (_selector2 != null)
                    return _internalSource.EntityQuery(context).Select(_selector2);
                return Enumerable.Empty<TResult>().AsQueryable();
            }
        }
    }
}