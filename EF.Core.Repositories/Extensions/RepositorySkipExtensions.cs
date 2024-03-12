using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySkipExtensions
    {
        public static IReadOnlyRepository<T> Skip<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new SkipRepository<T>(repository, count, false);
        }

        public static IReadOnlyRepository<T> SkipLast<T>(this IReadOnlyRepository<T> repository, int count)
        {
            return new SkipRepository<T>(repository, count, true);
        }

        public static IReadOnlyRepository<T> SkipWhile<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate)
        {
            return new SkipRepository<T>(repository, predicate);
        }

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