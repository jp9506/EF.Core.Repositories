using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// LINQ extension methods for <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class IReadOnlyRepositoryExtensions
    {
        /// <summary>
        /// Generates a string representation of the query used. This string may not be suitable for
        /// direct execution is intended only for use in debugging.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/>.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The query string for debugging.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<string> ToQueryStringAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(x => x.ToQueryString(), cancellationToken);

        #region Any/All

        /// <summary>
        /// Asynchronously determines whether all the elements of a repository satisfy a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to test for a condition.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if every element of the source repository passes the test in the
        /// specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<bool> AllAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AllAsync(predicate, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously determines whether a repository contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to check for being empty.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if the source repository contains any elements; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<bool> AnyAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AnyAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously determines whether any element of a repository satisfies a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> whose elements to test for a condition.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="true"/> if any elements in the source repository pass the test in the
        /// specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<bool> AnyAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AnyAsync(predicate, cancellationToken), cancellationToken);

        #endregion Any/All

        #region Count/LongCount

        /// <summary>
        /// Asynchronously returns the number of elements in a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to be counted.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number
        /// of elements in the input repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int> CountAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.CountAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the number of elements in a repository that satisfy a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to be counted.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number
        /// of elements in the repository that satisfy the condition in the predicate function.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int> CountAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.CountAsync(predicate, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns a <see cref="long"/> that represents the total number of elements
        /// in a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to be counted.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number
        /// of elements in the input repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long> LongCountAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LongCountAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns a <see cref="long"/> that represents the number of elements in a
        /// repository that satisfy a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to be counted.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number
        /// of elements in the repository that satisfy the condition in the predicate function.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long> LongCountAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LongCountAsync(predicate, cancellationToken), cancellationToken);

        #endregion Count/LongCount

        #region Get

        /// <summary>
        /// Asynchronously loads from an <see cref="IReadOnlyRepository{T}"/> by enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to create an array from.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an array
        /// that contains elements from the input repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<IEnumerable<T>> GetAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.ToArrayAsync(cancellationToken), cancellationToken);

        #endregion Get

        #region First/FirstOrDefault

        /// <summary>
        /// Asynchronously returns the first element of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the first element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first
        /// element in <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> FirstAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the first element of a repository that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the first element of.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first
        /// element in <paramref name="repository"/> that passes the test in <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>No element satisfies the condition in <paramref name="predicate"/></para>
        /// <para>-or -</para>
        /// <para><paramref name="repository"/> contains no elements.</para>
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> FirstAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstAsync(predicate, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the first element of a repository, or a default value if the
        /// repository contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the first element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="default"/> ( <typeparamref name="T"/> ) if <paramref name="repository"/> is
        /// empty; otherwise, the first element in <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> FirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstOrDefaultAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the first element of a repository that satisfies a specified
        /// condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the first element of.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="default"/> ( <typeparamref name="T"/> ) if <paramref name="repository"/> is
        /// empty or if no element passes the test specified by <paramref name="predicate"/>,
        /// otherwise, the first element in <paramref name="repository"/> that passes the test
        /// specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> FirstOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.FirstOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion First/FirstOrDefault

        #region Last/LastOrDefault

        /// <summary>
        /// Asynchronously returns the last element of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the last element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the last
        /// element in <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> LastAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the last element of a repository that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the last element of.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the last
        /// element in <paramref name="repository"/> that passes the test in <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>No element satisfies the condition in <paramref name="predicate"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="repository"/> contains no elements.</para>
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> LastAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastAsync(predicate, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the last element of a repository, or a default value if the
        /// repository contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the last element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="default"/> ( <typeparamref name="T"/> ) if <paramref name="repository"/> is
        /// empty; otherwise, the last element in <paramref name="repository"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> LastOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastOrDefaultAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the last element of a repository that satisfies a specified
        /// condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the last element of.
        /// </param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <see
        /// langword="default"/> ( <typeparamref name="T"/> ) if <paramref name="repository"/> is
        /// empty or if no element passes the test specified by <paramref name="predicate"/>,
        /// otherwise, the last element in <paramref name="repository"/> that passes the test
        /// specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> LastOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.LastOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion Last/LastOrDefault

        #region Single/SingleOrDefault

        /// <summary>
        /// Asynchronously returns the only element of a repository, and throws an exception if
        /// there is not exactly one element in the repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the single element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the single
        /// element of the input repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <para><paramref name="repository"/> contains more than one elements.</para>
        /// <para>-or-</para>
        /// <para><paramref name="repository"/> contains no elements.</para>
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> SingleAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the only element of a repository that satisfies a specified
        /// condition, and throws an exception if more than one such element exists.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the single element of.
        /// </param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the single
        /// element of the input repository that satisfies the condition in <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>No element satisfies the condition in <paramref name="predicate"/>.</para>
        /// <para>-or-</para>
        /// <para>More than one element satisfies the condition in <paramref name="predicate"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="repository"/> contains no elements.</para>
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> SingleAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleAsync(predicate, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the only element of a repository, or a default value if the
        /// repository is empty; this method throws an exception if there is more than one element
        /// in the repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the single element of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the single
        /// element of the input repository, or <see langword="default"/> ( <typeparamref
        /// name="T"/>) if the repository contains no elements.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains more than one element.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> SingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleOrDefaultAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously returns the only element of a repository that satisfies a specified
        /// condition or a default value if no such element exists; this method throws an exception
        /// if more than one element satisfies the condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> to return the single element of.
        /// </param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the single
        /// element of the input repository that satisfies the condition in <paramref
        /// name="predicate"/>, or <see langword="default"/> ( <typeparamref name="T"/> ) if no such
        /// element is found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// More than one element satisfies the condition in <paramref name="predicate"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T?> SingleOrDefaultAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SingleOrDefaultAsync(predicate, cancellationToken), cancellationToken);

        #endregion Single/SingleOrDefault

        #region Min

        /// <summary>
        /// Asynchronously returns the minimum value of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to determine the
        /// minimum of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the minimum
        /// value in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> MinAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MinAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously invokes a projection function on each element of a repository and returns
        /// the minimum resulting value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the value returned by the function represented by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to determine the
        /// minimum of.
        /// </param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the minimum
        /// value in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<TResult> MinAsync<T, TResult>(this IReadOnlyRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MinAsync(selector, cancellationToken), cancellationToken);

        #endregion Min

        #region Max

        /// <summary>
        /// Asynchronously returns the maximum value of a repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to determine the
        /// maximum of.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the maximum
        /// value in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<T> MaxAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MaxAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously invokes a projection function on each element of a repository and returns
        /// the maximum resulting value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the value returned by the function represented by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="repository">
        /// An <see cref="IReadOnlyRepository{T}"/> that contains the elements to determine the
        /// maximum of.
        /// </param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the maximum
        /// value in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<TResult> MaxAsync<T, TResult>(this IReadOnlyRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.MaxAsync(selector, cancellationToken), cancellationToken);

        #endregion Max

        #region Sum

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal> SumAsync(this IReadOnlyRepository<decimal> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal?> SumAsync(this IReadOnlyRepository<decimal?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int> SumAsync(this IReadOnlyRepository<int> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int?> SumAsync(this IReadOnlyRepository<int?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<int?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long> SumAsync(this IReadOnlyRepository<long> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long?> SumAsync(this IReadOnlyRepository<long?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<long?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> SumAsync(this IReadOnlyRepository<double> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> SumAsync(this IReadOnlyRepository<double?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float> SumAsync(this IReadOnlyRepository<float> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the sum of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the values in the repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float?> SumAsync(this IReadOnlyRepository<float?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float?>)repository).ExecuteAsync(async x => await x.SumAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the sum of the repository of values that is obtained by invoking
        /// a projection function on each element of the input repository.
        /// </summary>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the sum of
        /// the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float?> SumAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.SumAsync(selector, cancellationToken), cancellationToken);

        #endregion Sum

        #region Average

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal> AverageAsync(this IReadOnlyRepository<decimal> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal?> AverageAsync(this IReadOnlyRepository<decimal?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<decimal?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<decimal?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync(this IReadOnlyRepository<int> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync(this IReadOnlyRepository<int?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<int?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync(this IReadOnlyRepository<long> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync(this IReadOnlyRepository<long?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<long?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync(this IReadOnlyRepository<double> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync(this IReadOnlyRepository<double?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<double?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<double?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float> AverageAsync(this IReadOnlyRepository<float> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values.
        /// </summary>
        /// <param name="repository">A repository of values to calculate the average of.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the repository of values.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float?> AverageAsync(this IReadOnlyRepository<float?> repository, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<float?>)repository).ExecuteAsync(async x => await x.AverageAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="repository"/> contains no elements.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        /// <summary>
        /// Asynchronously computes the average of a repository of values that is obtained by
        /// invoking a projection function on each element of the input repository.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">A repository of values of type <typeparamref name="T"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average
        /// of the projected values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> or <paramref name="selector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async Task<float?> AverageAsync<T>(this IReadOnlyRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            => await ((IInternalReadOnlyRepository<T>)repository).ExecuteAsync(async x => await x.AverageAsync(selector, cancellationToken), cancellationToken);

        #endregion Average

        #region Execute

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalReadOnlyRepository<T> repository, Func<IQueryable<T>, TResult> expression, CancellationToken cancellationToken = default)
        {
            var ctx = await repository.Transaction.GetDbContextAsync(cancellationToken);
            return expression(repository.EntityQuery(ctx));
        }

        private static async Task<TResult> ExecuteAsync<T, TResult>(this IInternalReadOnlyRepository<T> repository, Func<IQueryable<T>, Task<TResult>> expression, CancellationToken cancellationToken = default)
        {
            var ctx = await repository.Transaction.GetDbContextAsync(cancellationToken);
            return await expression(repository.EntityQuery(ctx));
        }

        #endregion Execute
    }
}