using EF.Core.Repositories.Test.PostgreSQL.Container;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.PostgreSql;

namespace EF.Core.Repositories.Test.Extensions
{
    /// <summary>
    /// Extensions for configuring containers for a <see cref="IFactoryBuilder{TContext}"/>
    /// </summary>
    public static partial class IFactoryBuilderContainerExtensions
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="containerBuilderFunction"></param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> ConfigureContainerBuilder<TContext>(this IFactoryBuilder<TContext> builder, Func<PostgreSqlBuilder, PostgreSqlBuilder> containerBuilderFunction)
            where TContext : DbContext
        {
            SetConfigureBuilderFunc((PostgreSQLFactoryBuilder<TContext>)builder, containerBuilderFunction);
            return builder;
        }
    }
}