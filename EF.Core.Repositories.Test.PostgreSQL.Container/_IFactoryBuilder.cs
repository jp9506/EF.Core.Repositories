using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test
{
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        /// EntityFrameworkCore.PostgreSQL provider.
        /// </summary>
        /// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> Instance() => new PostgreSQL.Container.PostgreSQLFactoryBuilder<TContext>();
    }
}