using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    /// <summary>
    /// A builder for creating <see cref="IRepositoryFactory{TContext}"/> based upon a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of <see cref="DbContext"/> to be created by the factory.
    /// </typeparam>
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a <see cref="IRepositoryFactory{TContext}"/> for use in a test.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>The created <see cref="IRepositoryFactory{TContext}"/>.</returns>
        /// <exception cref="OperationCanceledException">
        /// If the <see cref="CancellationToken"/> is canceled.
        /// </exception>
        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}