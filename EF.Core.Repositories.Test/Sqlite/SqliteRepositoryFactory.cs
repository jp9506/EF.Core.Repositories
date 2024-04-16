using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EF.Core.Repositories.Test.Sqlite
{
    internal class SqliteRepositoryFactory<TContext> : RepositoryFactoryBase<TContext>
        where TContext : DbContext
    {
        private readonly DbConnection _connection;

        public SqliteRepositoryFactory()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(_connection)
                .Options;
        }

        protected override void OnDisposing()
        {
            _connection.Dispose();
        }
    }
}