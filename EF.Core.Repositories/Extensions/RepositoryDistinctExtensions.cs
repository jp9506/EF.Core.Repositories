using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryDistinctExtensions
    {
        public static IReadOnlyRepository<T> Distinct<T>(this IReadOnlyRepository<T> repository)
        {
            return new DistinctRepository<T>(repository);
        }

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