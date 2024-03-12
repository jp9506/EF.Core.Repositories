using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySelectExtensions
    {
        public static IReadOnlyRepository<TResult> Select<TSource, TResult>(this IReadOnlyRepository<TSource> repository, Expression<Func<TSource, TResult>> selector)
        {
            return new SelectRepository<TSource, TResult>(repository, selector);
        }

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