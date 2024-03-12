using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryWhereExtensions
    {
        public static IReadOnlyRepository<T> Where<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate)
        {
            return new WhereRepository<T>(repository, predicate);
        }

        public static IReadOnlyRepository<T> Where<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int, bool>> predicate)
        {
            return new WhereRepository<T>(repository, predicate);
        }

        private sealed class WhereRepository<T> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>
        {
            private readonly Expression<Func<T, bool>>? _predicate;
            private readonly Expression<Func<T, int, bool>>? _predicate2;

            public WhereRepository(IReadOnlyRepository<T> source, Expression<Func<T, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = predicate;
                _predicate2 = null;
            }

            public WhereRepository(IReadOnlyRepository<T> source, Expression<Func<T, int, bool>> predicate) : base((IInternalReadOnlyRepository<T>)source)
            {
                _predicate = null;
                _predicate2 = predicate;
            }

            public override IQueryable<T> EntityQuery(DbContext context)
            {
                if (_predicate != null)
                    return _internalSource.EntityQuery(context).Where(_predicate);
                if (_predicate2 != null)
                    return _internalSource.EntityQuery(context).Where(_predicate2);
                return Enumerable.Empty<T>().AsQueryable();
            }
        }
    }
}