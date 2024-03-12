using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryExceptExtensions
    {
        public static IReadOnlyRepository<T> Except<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new ExceptRepository<T>(source1, source2);
        }

        public static IReadOnlyRepository<T> Except<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new ExceptRepository<T>(source1, source2, comparer);
        }

        private sealed class ExceptRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<T>? _comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2;

            public ExceptRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _comparer = null;
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
            }

            public ExceptRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2,
                IEqualityComparer<T>? comparer) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _comparer = comparer;
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
            }

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