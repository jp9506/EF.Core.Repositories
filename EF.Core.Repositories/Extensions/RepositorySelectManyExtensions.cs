using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositorySelectManyExtensions
    {
        public static IReadOnlyRepository<TResult> SelectMany<TSource, TResult>(this IReadOnlyRepository<TSource> repository, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return new SelectManyRepository<TSource, TResult>(repository, selector);
        }

        public static IReadOnlyRepository<TResult> SelectMany<TSource, TResult>(this IReadOnlyRepository<TSource> repository, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
        {
            return new SelectManyRepository<TSource, TResult>(repository, selector);
        }

        private sealed class SelectManyRepository<TSource, TResult> : WrapperReadOnlyRepositoryBase<TSource, IInternalReadOnlyRepository<TSource>, TResult>
        {
            private readonly Expression<Func<TSource, IEnumerable<TResult>>>? _selector;
            private readonly Expression<Func<TSource, int, IEnumerable<TResult>>>? _selector2;

            public SelectManyRepository(IReadOnlyRepository<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector) : base((IInternalReadOnlyRepository<TSource>)source)
            {
                _selector = selector;
                _selector2 = null;
            }

            public SelectManyRepository(IReadOnlyRepository<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector) : base((IInternalReadOnlyRepository<TSource>)source)
            {
                _selector = null;
                _selector2 = selector;
            }

            public override IQueryable<TResult> EntityQuery(DbContext context)
            {
                if (_selector != null)
                    return _internalSource.EntityQuery(context).SelectMany(_selector);
                if (_selector2 != null)
                    return _internalSource.EntityQuery(context).SelectMany(_selector2);
                return Enumerable.Empty<TResult>().AsQueryable();
            }
        }
    }
}