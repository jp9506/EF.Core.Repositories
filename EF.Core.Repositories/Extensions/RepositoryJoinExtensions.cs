using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryJoinExtensions
    {
        public static IReadOnlyRepository<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IReadOnlyRepository<TOuter> outer,
            IReadOnlyRepository<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return new JoinRepository<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IReadOnlyRepository<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IReadOnlyRepository<TOuter> outer,
            IReadOnlyRepository<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new JoinRepository<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        private sealed class JoinRepository<TOuter, TInner, TKey, TResult> : WrapperReadOnlyRepositoryBase<TOuter, IInternalReadOnlyRepository<TOuter>, TResult>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Expression<Func<TInner, TKey>> _innerKeySelector;
            private readonly IInternalReadOnlyRepository<TInner> _internalInner;
            private readonly Expression<Func<TOuter, TKey>> _outerKeySelector;
            private readonly Expression<Func<TOuter, TInner, TResult>> _resultSelector;

            public JoinRepository(
                IReadOnlyRepository<TOuter> outer,
                IReadOnlyRepository<TInner> inner,
                Expression<Func<TOuter, TKey>> outerKeySelector,
                Expression<Func<TInner, TKey>> innerKeySelector,
                Expression<Func<TOuter, TInner, TResult>> resultSelector) : base((IInternalReadOnlyRepository<TOuter>)outer)
            {
                _innerKeySelector = innerKeySelector;
                _internalInner = (IInternalReadOnlyRepository<TInner>)inner;
                _outerKeySelector = outerKeySelector;
                _resultSelector = resultSelector;
                _comparer = null;
            }

            public JoinRepository(
                IReadOnlyRepository<TOuter> outer,
                IReadOnlyRepository<TInner> inner,
                Expression<Func<TOuter, TKey>> outerKeySelector,
                Expression<Func<TInner, TKey>> innerKeySelector,
                Expression<Func<TOuter, TInner, TResult>> resultSelector,
                IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<TOuter>)outer)
            {
                _innerKeySelector = innerKeySelector;
                _internalInner = (IInternalReadOnlyRepository<TInner>)inner;
                _outerKeySelector = outerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override IQueryable<TResult> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).Join(
                        _internalInner.EntityQuery(context),
                        _outerKeySelector,
                        _innerKeySelector,
                        _resultSelector);
                else
                    return _internalSource.EntityQuery(context).Join(
                        _internalInner.EntityQuery(context),
                        _outerKeySelector,
                        _innerKeySelector,
                        _resultSelector,
                        _comparer);
            }
        }
    }
}