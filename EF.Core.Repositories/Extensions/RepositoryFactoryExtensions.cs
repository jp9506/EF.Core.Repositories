using EF.Core.Repositories.Internal.Base;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IRepositoryFactory"/>.
    /// </summary>
    public static class RepositoryFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="ITransaction"/>.
        /// </summary>
        /// <returns>The created <see cref="ITransaction"/>.</returns>
        public static ITransaction CreateTransaction(this IRepositoryFactory repositoryFactory)
        {
            return new Transaction(repositoryFactory);
        }

        /// <summary>
        /// Gets a readonly repository for accessing data of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data in the repository.</typeparam>
        /// <returns>An <see cref="IReadOnlyRepository{T}"/>.</returns>
        public static IReadOnlyRepository<T> GetReadOnlyRepository<T>(this IRepositoryFactory repositoryFactory)
            where T : class
        {
            var transaction = new AutoTransaction(repositoryFactory);
            return transaction.GetReadOnlyRepository<T>();
        }

        /// <summary>
        /// Gets a read/write repository for accessing/updating data of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data in the repository.</typeparam>
        /// <returns>An <see cref="IRepository{T}"/>.</returns>
        public static IRepository<T> GetRepository<T>(this IRepositoryFactory repositoryFactory)
            where T : class
        {
            var transaction = new AutoTransaction(repositoryFactory);
            return transaction.GetRepository<T>();
        }

        internal sealed class AutoTransaction(IRepositoryFactory factory) : TransactionBase(factory)
        {
            public override bool AutoCommit => true;
        }

        internal sealed class Transaction(IRepositoryFactory factory) : TransactionBase(factory)
        {
            public override bool AutoCommit => false;
        }
    }
}