using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using Testcontainers.CosmosDb;

namespace EF.Core.Repositories.Test.Cosmos.Container
{
    internal class CosmosFactoryBuilder<TContext> : ContainerFactoryBuilderBase<TContext, CosmosDbBuilder, CosmosDbContainer, CosmosDbConfiguration>
        where TContext : DbContext
    {
        public CosmosFactoryBuilder()
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new CosmosRepositoryFactory<TContext>(GetHost());
    }
}