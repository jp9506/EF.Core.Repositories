using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test.InMemory
{
    internal class InMemoryFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        public InMemoryFactoryBuilder()
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new InMemoryRepositoryFactory<TContext>();
    }
}