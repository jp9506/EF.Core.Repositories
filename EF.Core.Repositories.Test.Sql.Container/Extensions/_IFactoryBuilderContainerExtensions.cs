using EF.Core.Repositories.Test.Sql.Container;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.MsSql;

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
        public static IFactoryBuilder<TContext> ConfigureContainerBuilder<TContext>(this IFactoryBuilder<TContext> builder, Func<MsSqlBuilder, MsSqlBuilder> containerBuilderFunction)
            where TContext : DbContext
        {
            SetConfigureBuilderFunc((SqlFactoryBuilder<TContext>)builder, containerBuilderFunction);
            return builder;
        }
    }
}