using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.CosmosDb;

namespace EF.Core.Repositories.Test.Cosmos.Container
{
    internal class CosmosRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        public CosmosRepositoryFactory(CosmosDbContainer host)
        {
            var id = Guid.NewGuid().ToString().ToLower().Replace('-', '.');

            _options = new DbContextOptionsBuilder<TContext>()
                .UseCosmos(host.GetConnectionString(), $"{typeof(TContext).Name}.{id}")
                .Options;
        }

        protected override void OnDisposing()
        {
            var ctx = GetDbContextAsync().Result;
            ctx.Database.EnsureDeleted();
        }
    }
}