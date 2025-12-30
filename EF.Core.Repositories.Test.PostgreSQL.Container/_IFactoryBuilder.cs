using EF.Core.Repositories.Test.PostgreSQL.Container;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.PostgreSql;

namespace EF.Core.Repositories.Test
{
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Globally configures the <see cref="PostgreSqlBuilder"/> for use in tests.
        /// </summary>
        /// <param name="containerBuilderFunction">The configurer for the <see cref="PostgreSqlBuilder"/></param>
        public static void ConfigureContainerBuilder(Func<PostgreSqlBuilder, PostgreSqlBuilder> containerBuilderFunction)
        {
            PostgreSQLFactoryBuilder<TContext>.SetConfigureBuilderFunc(containerBuilderFunction);
        }

        /// <summary>
        /// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        /// EntityFrameworkCore.PostgreSQL provider.
        /// </summary>
        /// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> Instance() => new PostgreSQLFactoryBuilder<TContext>();
    }
}