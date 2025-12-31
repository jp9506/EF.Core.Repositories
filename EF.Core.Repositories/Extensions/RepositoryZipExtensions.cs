using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Zip extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryZipExtensions
    {
        /// <summary>
        /// Produces a repository of tuples with elements from the two specified repositories.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input repository.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input repository.</typeparam>
        /// <param name="source1">The first repository to merge.</param>
        /// <param name="source2">The second repository to merge.</param>
        /// <returns>
        /// A repository of tuples with elements taken from the first and second repositories, in
        /// that order.
        /// </returns>
        public static IReadOnlyRepository<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2)
        {
            return new ZipRepository<TFirst, TSecond>(source1, source2);
        }

        /// <summary>
        /// Merges two repositories by using the specified predicate function.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input repository.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input repository.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result repository.</typeparam>
        /// <param name="source1">The first repository to merge.</param>
        /// <param name="source2">The second repository to merge.</param>
        /// <param name="resultSelector">
        /// A function that specifies how to merge the elements from the two repositories.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{TResult}"/> that contains merged elements of two input repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<TResult> Zip<TFirst, TSecond, TResult>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2,
            Expression<Func<TFirst, TSecond, TResult>> resultSelector)
        {
            return new ZipRepository<TFirst, TSecond, TResult>(source1, source2, resultSelector);
        }

        /// <summary>
        /// Produces a repository of tuples with elements from the three specified repositories.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input repository.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input repository.</typeparam>
        /// <typeparam name="TThird">The type of the elements of the third input repository.</typeparam>
        /// <param name="source1">The first repository to merge.</param>
        /// <param name="source2">The second repository to merge.</param>
        /// <param name="source3">The third repository to merge.</param>
        /// <returns>
        /// A repository of tuples with elements taken from the first, second and third
        /// repositories, in that order.
        /// </returns>
        public static IReadOnlyRepository<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2,
            IReadOnlyRepository<TThird> source3)
        {
            return new ZipRepository3<TFirst, TSecond, TThird>(source1, source2, source3);
        }

        private sealed class ZipRepository<TFirst, TSecond>(IReadOnlyRepository<TFirst> source1, IReadOnlyRepository<TSecond> source2)
            : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, (TFirst First, TSecond Second)>((IInternalReadOnlyRepository<TFirst>)source1)
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;

            public override IQueryable<(TFirst First, TSecond Second)> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context));
            }
        }

        private sealed class ZipRepository<TFirst, TSecond, TResult>(IReadOnlyRepository<TFirst> source1, IReadOnlyRepository<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
            : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, TResult>((IInternalReadOnlyRepository<TFirst>)source1)
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;
            private readonly Expression<Func<TFirst, TSecond, TResult>> _resultSelector = resultSelector;

            public override IQueryable<TResult> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context), _resultSelector);
            }
        }

        private sealed class ZipRepository3<TFirst, TSecond, TThird>(IReadOnlyRepository<TFirst> source1, IReadOnlyRepository<TSecond> source2, IReadOnlyRepository<TThird> source3)
            : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, (TFirst First, TSecond Second, TThird Third)>((IInternalReadOnlyRepository<TFirst>)source1)
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;
            private readonly IInternalReadOnlyRepository<TThird> _internalSource3 = (IInternalReadOnlyRepository<TThird>)source3;

            public override IQueryable<(TFirst First, TSecond Second, TThird Third)> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context), _internalSource3.EntityQuery(context));
            }
        }
    }
}