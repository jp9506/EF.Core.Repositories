using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Relational extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryRelationalExtensions
    {
        /// <summary>
        /// Returns a new repository which is configured to load the collections in the query results in a single database query.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">The source <see cref="IReadOnlyRepository{T}"/>.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> where collections will be loaded from <paramref name="repository"/> through a single query.
        /// </returns>
        public static IReadOnlyRepository<T> AsSingleQuery<T>(this IReadOnlyRepository<T> repository)
            where T : class
        {
            return new AsSingleQueryRepository<T>(repository);
        }

        /// <summary>
        /// Returns a new repository which is configured to load the collections in the query results through separate database queries.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">The source <see cref="IReadOnlyRepository{T}"/>.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> where collections will be loaded from <paramref name="repository"/> through separate database queries.
        /// </returns>
        public static IReadOnlyRepository<T> AsSplitQuery<T>(this IReadOnlyRepository<T> repository)
            where T : class
        {
            return new AsSplitQueryRepository<T>(repository);
        }

        private sealed class AsSingleQueryRepository<T>(IReadOnlyRepository<T> source) : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source)
            where T : class
        {
            public override IQueryable<T> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).AsSingleQuery();
            }
        }

        private sealed class AsSplitQueryRepository<T>(IReadOnlyRepository<T> source) : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>((IInternalReadOnlyRepository<T>)source)
                    where T : class
        {
            public override IQueryable<T> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).AsSplitQuery();
            }
        }
    }
}