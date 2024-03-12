using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryThenByExtensions
    {
        public static IOrderedRepository<T> ThenBy<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, true);
        }

        public static IOrderedRepository<T> ThenBy<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, comparer, true);
        }

        public static IOrderedRepository<T> ThenByDescending<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, false);
        }

        public static IOrderedRepository<T> ThenByDescending<T, TKey>(this IOrderedRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new ThenOrderedRepository<T, TKey>(repository, keySelector, comparer, false);
        }

        private sealed class ThenOrderedRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalOrderedRepository<T>>, IInternalOrderedRepository<T>
        {
            private readonly bool _ascending;
            private readonly IComparer<TKey>? _comparer;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public ThenOrderedRepository(IOrderedRepository<T> source, Expression<Func<T, TKey>> keySelector, bool ascending) : base((IInternalOrderedRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = null;
            }

            public ThenOrderedRepository(IOrderedRepository<T> source, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer, bool ascending) : base((IInternalOrderedRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = comparer;
            }

            public IOrderedQueryable<T> EntityOrdered(DbContext context)
            {
                if (_comparer == null)
                    return _ascending
                        ? _internalSource.EntityOrdered(context).ThenBy(_keySelector)
                        : _internalSource.EntityOrdered(context).ThenByDescending(_keySelector);
                else
                    return _ascending
                        ? _internalSource.EntityOrdered(context).ThenBy(_keySelector, _comparer)
                        : _internalSource.EntityOrdered(context).ThenByDescending(_keySelector, _comparer);
            }

            public override IQueryable<T> EntityQuery(DbContext context) => EntityOrdered(context);
        }
    }
}