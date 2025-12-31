using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test
{
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.Sqlite provider.
        /// </summary>
        /// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> Instance() => new Sqlite.SqliteFactoryBuilder<TContext>();
    }
}