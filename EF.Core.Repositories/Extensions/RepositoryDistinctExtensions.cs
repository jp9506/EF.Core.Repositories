using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryDistinctExtensions
    {
        /// <summary>
        /// Returns distinct elements from a repository using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to remove duplicates from.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains distinct elements from <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        public static IReadOnlyRepository<T> Distinct<T>(this IReadOnlyRepository<T> repository)
        {
            return new DistinctRepository<T>(repository);
        }

        /// <summary>
        /// Returns distinct elements from a repository using a specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// The <see cref="IReadOnlyRepository{T}"/> to remove duplicates from.
        /// </param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that contains distinct elements from <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        public static IReadOnlyRepository<T> Distinct<T>(this IReadOnlyRepository<T> repository, IEqualityComparer<T>? comparer)
        {
            return new DistinctRepository<T>(repository, comparer);
        }

        private sealed class DistinctRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<T>? _comparer;

            public DistinctRepository(IReadOnlyRepository<T> source) : base((IInternalReadOnlyRepository<T>)source)
            {
                _comparer = null;
            }

            public DistinctRepository(IReadOnlyRepository<T> source, IEqualityComparer<T>? comparer) : base((IInternalReadOnlyRepository<T>)source)
            {
                _comparer = comparer;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Distinct();
                else
                    return _internalSource.EntityQuery(context).Distinct(_comparer);
            }
        }
    }
}