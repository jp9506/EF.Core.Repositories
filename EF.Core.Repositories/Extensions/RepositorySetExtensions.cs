using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySetExtensions
    {
        public static IReadOnlyRepository<T> Intersect<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new IntersectRepository<T>(source1, source2);
        }

        public static IReadOnlyRepository<T> Intersect<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new IntersectRepository<T>(source1, source2, comparer);
        }

        public static IReadOnlyRepository<T> IntersectBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<TKey> source2,
            Expression<Func<T, TKey>> keySelector)
        {
            return new IntersectByRepository<T, TKey>(source1, source2, keySelector);
        }

        public static IReadOnlyRepository<T> IntersectBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<TKey> source2,
            Expression<Func<T, TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new IntersectByRepository<T, TKey>(source1, source2, keySelector, comparer);
        }

        public static IReadOnlyRepository<T> Union<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2)
        {
            return new UnionRepository<T>(source1, source2);
        }

        public static IReadOnlyRepository<T> Union<T>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            IEqualityComparer<T>? comparer)
        {
            return new UnionRepository<T>(source1, source2, comparer);
        }

        public static IReadOnlyRepository<T> UnionBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            Expression<Func<T, TKey>> keySelector)
        {
            return new UnionByRepository<T, TKey>(source1, source2, keySelector);
        }

        public static IReadOnlyRepository<T> UnionBy<T, TKey>(
            this IReadOnlyRepository<T> source1,
            IReadOnlyRepository<T> source2,
            Expression<Func<T, TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new UnionByRepository<T, TKey>(source1, source2, keySelector, comparer);
        }

        private sealed class IntersectByRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly IInternalReadOnlyRepository<TKey> _internalSource2;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public IntersectByRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<TKey> source2,
                Expression<Func<T, TKey>> keySelector) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<TKey>)source2;
                _keySelector = keySelector;
                _comparer = null;
            }

            public IntersectByRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<TKey> source2,
                Expression<Func<T, TKey>> keySelector,
                IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<TKey>)source2;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).IntersectBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector);
                else
                    return _internalSource.EntityQuery(context).IntersectBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector,
                        _comparer);
            }
        }

        private sealed class IntersectRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<T>? _comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2;

            public IntersectRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _comparer = null;
            }

            public IntersectRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2,
                IEqualityComparer<T>? comparer) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _comparer = comparer;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Intersect(
                        _internalSource2.EntityQuery(context));
                else
                    return _internalSource.EntityQuery(context).Intersect(
                        _internalSource2.EntityQuery(context),
                        _comparer);
            }
        }

        private sealed class UnionByRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public UnionByRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2,
                Expression<Func<T, TKey>> keySelector) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _keySelector = keySelector;
                _comparer = null;
            }

            public UnionByRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2,
                Expression<Func<T, TKey>> keySelector,
                IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).UnionBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector);
                else
                    return _internalSource.EntityQuery(context).UnionBy(
                        _internalSource2.EntityQuery(context),
                        _keySelector,
                        _comparer);
            }
        }

        private sealed class UnionRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly IEqualityComparer<T>? _comparer;
            private readonly IInternalReadOnlyRepository<T> _internalSource2;

            public UnionRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _comparer = null;
            }

            public UnionRepository(
                IReadOnlyRepository<T> source1,
                IReadOnlyRepository<T> source2,
                IEqualityComparer<T>? comparer) : base((IInternalReadOnlyRepository<T>)source1)
            {
                _internalSource2 = (IInternalReadOnlyRepository<T>)source2;
                _comparer = comparer;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Union(
                        _internalSource2.EntityQuery(context));
                else
                    return _internalSource.EntityQuery(context).Union(
                        _internalSource2.EntityQuery(context),
                        _comparer);
            }
        }
    }
}