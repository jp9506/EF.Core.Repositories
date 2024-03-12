using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryZipExtensions
    {
        public static IReadOnlyRepository<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2)
        {
            return new ZipRepository<TFirst, TSecond>(source1, source2);
        }

        public static IReadOnlyRepository<TResult> Zip<TFirst, TSecond, TResult>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2,
            Expression<Func<TFirst, TSecond, TResult>> resultSelector)
        {
            return new ZipRepository<TFirst, TSecond, TResult>(source1, source2, resultSelector);
        }

        public static IReadOnlyRepository<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(
            this IReadOnlyRepository<TFirst> source1,
            IReadOnlyRepository<TSecond> source2,
            IReadOnlyRepository<TThird> source3)
        {
            return new ZipRepository3<TFirst, TSecond, TThird>(source1, source2, source3);
        }

        private sealed class ZipRepository<TFirst, TSecond> : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, (TFirst First, TSecond Second)>
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2;

            public ZipRepository(
                IReadOnlyRepository<TFirst> source1,
                IReadOnlyRepository<TSecond> source2) : base((IInternalReadOnlyRepository<TFirst>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;
            }

            public override IQueryable<(TFirst First, TSecond Second)> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context));
            }
        }

        private sealed class ZipRepository<TFirst, TSecond, TResult> : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, TResult>
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2;
            private readonly Expression<Func<TFirst, TSecond, TResult>> _resultSelector;

            public ZipRepository(
                IReadOnlyRepository<TFirst> source1,
                IReadOnlyRepository<TSecond> source2,
                Expression<Func<TFirst, TSecond, TResult>> resultSelector) : base((IInternalReadOnlyRepository<TFirst>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;
                _resultSelector = resultSelector;
            }

            public override IQueryable<TResult> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context), _resultSelector);
            }
        }

        private sealed class ZipRepository3<TFirst, TSecond, TThird> : WrapperReadOnlyRepositoryBase<TFirst, IInternalReadOnlyRepository<TFirst>, (TFirst First, TSecond Second, TThird Third)>
        {
            private readonly IInternalReadOnlyRepository<TSecond> _internalSource2;
            private readonly IInternalReadOnlyRepository<TThird> _internalSource3;

            public ZipRepository3(
                IReadOnlyRepository<TFirst> source1,
                IReadOnlyRepository<TSecond> source2,
                IReadOnlyRepository<TThird> source3) : base((IInternalReadOnlyRepository<TFirst>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<TSecond>)source2;
                _internalSource3 = (IInternalReadOnlyRepository<TThird>)source3;
            }

            public override IQueryable<(TFirst First, TSecond Second, TThird Third)> EntityQuery(DbContext context)
            {
                return _internalSource.EntityQuery(context).Zip(_internalSource2.EntityQuery(context), _internalSource3.EntityQuery(context));
            }
        }
    }
}