using EF.Core.Repositories.Test.Cosmos.Container;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.CosmosDb;

namespace EF.Core.Repositories.Test
{
    public partial interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Globally configures the <see cref="CosmosDbBuilder"/> for use in tests.
        /// </summary>
        /// <param name="containerBuilderFunction">The configurer for the <see cref="CosmosDbBuilder"/></param>
        public static void ConfigureContainerBuilder(Func<CosmosDbBuilder, CosmosDbBuilder> containerBuilderFunction)
        {
            CosmosFactoryBuilder<TContext>.SetConfigureBuilderFunc(containerBuilderFunction);
        }

        /// <summary>
        /// Creates a <see cref="IFactoryBuilder{TContext}"/> using the EntityFrameworkCore.Cosmos provider.
        /// </summary>
        /// <returns>The created <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> Instance() => new CosmosFactoryBuilder<TContext>();
    }
}