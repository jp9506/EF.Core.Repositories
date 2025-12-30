using EF.Core.Repositories.Test.Sql.Container;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.MsSql;

namespace EF.Core.Repositories.Test
{
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Globally configures the <see cref="MsSqlBuilder"/> for use in tests.
        /// </summary>
        /// <param name="containerBuilderFunction">The configurer for the <see cref="MsSqlBuilder"/></param>
        public static void ConfigureContainerBuilder(Func<MsSqlBuilder, MsSqlBuilder> containerBuilderFunction)
        {
            SqlFactoryBuilder<TContext>.SetConfigureBuilderFunc(containerBuilderFunction);
        }

        /// <summary>
        /// Creates a <see cref="IFactoryBuilder{TContext}"/> using the
        /// EntityFrameworkCore.SqlServer provider.
        /// </summary>
        /// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> Instance() => new SqlFactoryBuilder<TContext>();
    }
}