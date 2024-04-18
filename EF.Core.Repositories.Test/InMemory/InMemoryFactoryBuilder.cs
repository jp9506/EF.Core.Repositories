using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.InMemory
{
    internal class InMemoryFactoryBuilder<TContext> : FactoryBuilderBase<TContext>
        where TContext : DbContext
    {
        public InMemoryFactoryBuilder(Func<IEnumerable<object>> seedFunction) : base(seedFunction)
        { }

        public InMemoryFactoryBuilder(Func<Task<IEnumerable<object>>> seedFunction) : base(seedFunction)
        { }

        public InMemoryFactoryBuilder(Func<CancellationToken, Task<IEnumerable<object>>> seedFunction) : base(seedFunction)
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new InMemoryRepositoryFactory<TContext>();
    }
}