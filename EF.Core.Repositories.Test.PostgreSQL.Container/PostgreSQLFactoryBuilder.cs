using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace EF.Core.Repositories.Test.PostgreSQL.Container
{
    internal class PostgreSQLFactoryBuilder<TContext> : ContainerFactoryBuilderBase<TContext, PostgreSqlBuilder, PostgreSqlContainer, PostgreSqlConfiguration>
        where TContext : DbContext
    {
        public PostgreSQLFactoryBuilder()
        { }

        protected override IRepositoryFactory<TContext> GetFactory() => new PostgreSQLRepositoryFactory<TContext>(GetHost());
    }
}