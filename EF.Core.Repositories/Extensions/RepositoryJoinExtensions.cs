using EF.Core.Repositories.Internal.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Join extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryJoinExtensions
    {
        /// <summary>
        /// Correlates the elements of two repositories based on matching keys using the default
        /// equality comparer.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of <paramref name="outer"/>.</typeparam>
        /// <typeparam name="TInner">The type of the elements of <paramref name="inner"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the functions represented by <paramref
        /// name="outerKeySelector"/> and <paramref name="innerKeySelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first repository to join.</param>
        /// <param name="inner">The repository to join to <paramref name="outer"/>.</param>
        /// <param name="outerKeySelector">
        /// A function to extract the join key from each element of <paramref name="outer"/>.
        /// </param>
        /// <param name="innerKeySelector">
        /// A function to extract the join key from each element of <paramref name="inner"/>.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result element from two matching elements.
        /// </param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <typeparamref
        /// name="TResult"/> obtained by performing an inner join on two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outer"/> or <paramref name="inner"/> or <paramref
        /// name="outerKeySelector"/> or <paramref name="innerKeySelector"/> or <paramref
        /// name="resultSelector"/> is <see langword="null"/>.
        /// </exception>
        public static IReadOnlyRepository<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IReadOnlyRepository<TOuter> outer,
            IReadOnlyRepository<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return new JoinRepository<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Correlates the elements of two repositories based on matching keys using the specified
        /// <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of <paramref name="outer"/>.</typeparam>
        /// <typeparam name="TInner">The type of the elements of <paramref name="inner"/>.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the functions represented by <paramref
        /// name="outerKeySelector"/> and <paramref name="innerKeySelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first repository to join.</param>
        /// <param name="inner">The repository to join to <paramref name="outer"/>.</param>
        /// <param name="outerKeySelector">
        /// A function to extract the join key from each element of <paramref name="outer"/>.
        /// </param>
        /// <param name="innerKeySelector">
        /// A function to extract the join key from each element of <paramref name="inner"/>.
        /// </param>
        /// <param name="resultSelector">
        /// A function to create a result element from two matching elements.
        /// </param>
        /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
        /// <returns>
        /// An <see cref="IReadOnlyRepository{T}"/> that has elements of type <typeparamref
        /// name="TResult"/> obtained by performing an inner join on two repositories.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outer"/> or <paramref name="inner"/> or <paramref
        /// name="outerKeySelector"/> or <paramref name="innerKeySelector"/> or <paramref
        /// name="resultSelector"/> or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
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

        private sealed class JoinRepository<TOuter, TInner, TKey, TResult>(IReadOnlyRepository<TOuter> outer, IReadOnlyRepository<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer = null)
            : WrapperReadOnlyRepositoryBase<TOuter, IInternalReadOnlyRepository<TOuter>, TResult>((IInternalReadOnlyRepository<TOuter>)outer)
        {
            private readonly IEqualityComparer<TKey>? _comparer = comparer;
            private readonly Expression<Func<TInner, TKey>> _innerKeySelector = innerKeySelector;
            private readonly IInternalReadOnlyRepository<TInner> _internalInner = (IInternalReadOnlyRepository<TInner>)inner;
            private readonly Expression<Func<TOuter, TKey>> _outerKeySelector = outerKeySelector;
            private readonly Expression<Func<TOuter, TInner, TResult>> _resultSelector = resultSelector;

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