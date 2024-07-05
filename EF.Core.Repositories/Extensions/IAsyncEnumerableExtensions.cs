using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Extensions for creating <see cref="IAsyncEnumerable{T}"/> from a <see cref="IReadOnlyRepository{T}"/>
    /// </summary>
    public static class IAsyncEnumerableExtensions
    {
        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}"/> from an <see cref="IReadOnlyRepository{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="repository"/>.</typeparam>
        /// <param name="repository">An <see cref="IReadOnlyRepository{T}"/> to retrieve data from.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while enumerating the <see
        /// cref="IReadOnlyRepository{T}"/> asynchronously.
        /// </param>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input repository.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IReadOnlyRepository<T> repository, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var items = await repository.GetAsync(cancellationToken);
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}