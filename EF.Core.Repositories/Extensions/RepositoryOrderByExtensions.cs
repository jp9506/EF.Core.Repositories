using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryOrderByExtensions
    {
        public static IOrderedRepository<T> OrderBy<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, true);
        }

        public static IOrderedRepository<T> OrderBy<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, comparer, true);
        }

        public static IOrderedRepository<T> OrderByDescending<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, false);
        }

        public static IOrderedRepository<T> OrderByDescending<T, TKey>(this IReadOnlyRepository<T> repository, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            return new OrderedRepository<T, TKey>(repository, keySelector, comparer, false);
        }

        private sealed class OrderedRepository<T, TKey> : WrapperReadOnlyRepositoryBase<T, IInternalReadOnlyRepository<T>>, IInternalOrderedRepository<T>
        {
            private readonly bool _ascending;
            private readonly IComparer<TKey>? _comparer;
            private readonly Expression<Func<T, TKey>> _keySelector;

            public OrderedRepository(IReadOnlyRepository<T> source, Expression<Func<T, TKey>> keySelector, bool ascending) : base((IInternalReadOnlyRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = null;
            }

            public OrderedRepository(IReadOnlyRepository<T> source, Expression<Func<T, TKey>> keySelector, IComparer<TKey>? comparer, bool ascending) : base((IInternalReadOnlyRepository<T>)source)
            {
                _keySelector = keySelector;
                _ascending = ascending;
                _comparer = comparer;
            }

            public IOrderedQueryable<T> EntityOrdered(DbContext context)
            {
                if (_comparer == null)
                    return _ascending
                        ? _internalSource.EntityQuery(context).OrderBy(_keySelector)
                        : _internalSource.EntityQuery(context).OrderByDescending(_keySelector);
                else
                    return _ascending
                        ? _internalSource.EntityQuery(context).OrderBy(_keySelector, _comparer)
                        : _internalSource.EntityQuery(context).OrderByDescending(_keySelector, _comparer);
            }

            public override IQueryable<T> EntityQuery(DbContext context) => EntityOrdered(context);
        }
    }
}