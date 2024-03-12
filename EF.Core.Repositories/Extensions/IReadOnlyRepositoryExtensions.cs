using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Extensions
{
    public static class IReadOnlyRepositoryExtensions
    {
        public static async Task<string> ToQueryStringAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(x => x.ToQueryString(), cancellationToken);

        #region Any/All

        public static async Task<bool> AllAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AllAsync(predicate, cancellationToken), cancellationToken);

        public static async Task<bool> AnyAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AnyAsync(cancellationToken), cancellationToken);

        public static async Task<bool> AnyAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AnyAsync(predicate, cancellationToken), cancellationToken);

        #endregion Any/All

        #region Count/LongCount

        public static async Task<int> CountAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.CountAsync(cancellationToken), cancellationToken);

        public static async Task<int> CountAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.CountAsync(predicate, cancellationToken), cancellationToken);

        public static async Task<long> LongCountAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LongCountAsync(cancellationToken), cancellationToken);

        public static async Task<long> LongCountAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LongCountAsync(predicate, cancellationToken), cancellationToken);

        #endregion Count/LongCount

        #region Get

        public static async Task<IEnumerable<T>> GetAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.ToArrayAsync(cancellationToken), cancellationToken);

        #endregion Get

        #region First/FirstOrDefault

        public static async Task<T> FirstAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstAsync(cancellationToken), cancellationToken);

        public static async Task<T> FirstAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstAsync(predicate, cancellationToken), cancellationToken);

        public static async Task<T?> FirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstOrDefaultAsync(cancellationToken), cancellationToken);

        public static async Task<T?> FirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion First/FirstOrDefault

        #region Last/LastOrDefault

        public static async Task<T> LastAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastAsync(cancellationToken), cancellationToken);

        public static async Task<T> LastAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastAsync(predicate, cancellationToken), cancellationToken);

        public static async Task<T?> LastOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastOrDefaultAsync(cancellationToken), cancellationToken);

        public static async Task<T?> LastOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion Last/LastOrDefault

        #region Single/SingleOrDefault

        public static async Task<T> SingleAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleAsync(cancellationToken), cancellationToken);

        public static async Task<T> SingleAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleAsync(predicate, cancellationToken), cancellationToken);

        public static async Task<T?> SingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleOrDefaultAsync(cancellationToken), cancellationToken);

        public static async Task<T?> SingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion Single/SingleOrDefault

        #region Min

        public static async Task<T> MinAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MinAsync(cancellationToken), cancellationToken);

        public static async Task<TResult> MinAsync<T, TResult>(this IReadOnlyRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MinAsync(selector, cancellationToken), cancellationToken);

        #endregion Min

        #region Max

        public static async Task<T> MaxAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MaxAsync(cancellationToken), cancellationToken);

        public static async Task<TResult> MaxAsync<T, TResult>(this IReadOnlyRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MaxAsync(selector, cancellationToken), cancellationToken);

        #endregion Max

        #region Sum

        public static async Task<decimal> SumAsync(this IReadOnlyRepository<decimal> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<decimal?> SumAsync(this IReadOnlyRepository<decimal?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<decimal> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<decimal?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<int> SumAsync(this IReadOnlyRepository<int> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<int?> SumAsync(this IReadOnlyRepository<int?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<int> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<int?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<long> SumAsync(this IReadOnlyRepository<long> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<long?> SumAsync(this IReadOnlyRepository<long?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<long> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<long?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double> SumAsync(this IReadOnlyRepository<double> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<double?> SumAsync(this IReadOnlyRepository<double?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<double> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<float> SumAsync(this IReadOnlyRepository<float> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<float?> SumAsync(this IReadOnlyRepository<float?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        public static async Task<float> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        public static async Task<float?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        #endregion Sum

        #region Average

        public static async Task<decimal> AverageAsync(this IReadOnlyRepository<decimal> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<decimal?> AverageAsync(this IReadOnlyRepository<decimal?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<decimal> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<decimal?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync(this IReadOnlyRepository<int> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync(this IReadOnlyRepository<int?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync(this IReadOnlyRepository<long> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync(this IReadOnlyRepository<long?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync(this IReadOnlyRepository<double> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync(this IReadOnlyRepository<double?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<float> AverageAsync(this IReadOnlyRepository<float> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<float?> AverageAsync(this IReadOnlyRepository<float?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        public static async Task<float> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        public static async Task<float?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        #endregion Average

        #region Execute

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalReadOnlyRepository<T> repository, Func<IQueryable<T>, TResult> expression, CancellationToken cancellationToken = default)
        {
            using var ctx = await repository.Factory.CreateDbContextAsync(cancellationToken);
            return expression(repository.EntityQuery(ctx));
        }

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalReadOnlyRepository<T> repository, Func<IQueryable<T>, Task<TResult>> expression, CancellationToken cancellationToken = default)
        {
            using var ctx = await repository.Factory.CreateDbContextAsync(cancellationToken);
            return await expression(repository.EntityQuery(ctx));
        }

        #endregion Execute
    }
}