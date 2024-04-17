using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.InMemory
{
    internal class InMemoryFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        public InMemoryFactoryBuilder(Action<TContext> contextAction) : base(contextAction)
        { }

        public InMemoryFactoryBuilder(Func<TContext, Task> contextAction) : base(contextAction)
        { }

        public InMemoryFactoryBuilder(Func<TContext, CancellationToken, Task> contextAction) : base(contextAction)
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new InMemoryRepositoryFactory<TContext>();
    }
}