using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class RepositoryGroupByExtensions
    {
        public static IReadOnlyRepository<IGrouping<TKey, TEntity>> GroupBy<TEntity, TKey>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector)
        {
            return new GroupByRepository<TEntity, TKey>(repository, keySelector);
        }

        public static IReadOnlyRepository<IGrouping<TKey, TEntity>> GroupBy<TEntity, TKey>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return new GroupByRepository<TEntity, TKey>(repository, keySelector, comparer);
        }

        public static IReadOnlyRepository<IGrouping<TKey, TElement>> GroupBy<TEntity, TKey, TElement>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector)
        {
            return new GroupByRepository<TEntity, TKey, TElement>(repository, keySelector, elementSelector);
        }

        public static IReadOnlyRepository<IGrouping<TKey, TElement>> GroupBy<TEntity, TKey, TElement>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            return new GroupByRepository<TEntity, TKey, TElement>(repository, keySelector, elementSelector, comparer);
        }

        private sealed class GroupByRepository<TEntity, TKey> : WrapperReadOnlyRepositoryBase<TEntity, IInternalReadOnlyRepository<TEntity>, IGrouping<TKey, TEntity>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Expression<Func<TEntity, TKey>> _keySelector;

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = null;
                _keySelector = keySelector;
            }

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = comparer;
                _keySelector = keySelector;
            }

            public override IQueryable<IGrouping<TKey, TEntity>> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector);
                else
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _comparer);
            }
        }

        private sealed class GroupByRepository<TEntity, TKey, TElement> : WrapperReadOnlyRepositoryBase<TEntity, IInternalReadOnlyRepository<TEntity>, IGrouping<TKey, TElement>>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Expression<Func<TEntity, TElement>> _elementSelector;
            private readonly Expression<Func<TEntity, TKey>> _keySelector;

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = null;
                _elementSelector = elementSelector;
                _keySelector = keySelector;
            }

            public GroupByRepository(IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TElement>> elementSelector, IEqualityComparer<TKey>? comparer) : base((IInternalReadOnlyRepository<TEntity>)source)
            {
                _comparer = comparer;
                _elementSelector = elementSelector;
                _keySelector = keySelector;
            }

            public override IQueryable<IGrouping<TKey, TElement>> EntityQuery(DbContext context)
            {
                if (_comparer == null)
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _elementSelector);
                else
                    return _internalSource.EntityQuery(context).GroupBy(_keySelector, _elementSelector, _comparer);
            }
        }
    }
}