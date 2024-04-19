using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace EF.Core.Repositories.Test.InMemory
{
    internal class InMemoryRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        private readonly InMemoryDatabaseRoot _databaseRoot;

        public InMemoryRepositoryFactory()
        {
            _databaseRoot = new InMemoryDatabaseRoot();
            _options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(typeof(TContext).FullName!, _databaseRoot)
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
    }
}