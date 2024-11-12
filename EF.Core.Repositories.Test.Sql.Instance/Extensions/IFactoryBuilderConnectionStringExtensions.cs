using EF.Core.Repositories.Test.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Extensions
{
    /// <summary>
    /// Extensions for specifying connection string info to a <see cref="IFactoryBuilder{TContext}"/>
    /// </summary>
    public static class IFactoryBuilderConnectionStringExtensions
    {
        /// <summary>
        /// Specifies a connection string for the <see cref="IFactoryBuilder{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="connStringBuilder">
        /// A <see cref="SqlConnectionStringBuilder"/> to be used to create backing databases.
        /// </param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> WithConnectionString<TContext>(this IFactoryBuilder<TContext> builder, SqlConnectionStringBuilder connStringBuilder)
            where TContext : DbContext
        {
            SetConnectionString((FactoryBuilderBase<TContext>)builder, connStringBuilder);
            return builder;
        }

        /// <summary>
        /// Specifies a connection string for the <see cref="IFactoryBuilder{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="connectionString">A connection string to be used to create backing databases.</param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> WithConnectionString<TContext>(this IFactoryBuilder<TContext> builder, string connectionString)
            where TContext : DbContext
        {
            SetConnectionString((FactoryBuilderBase<TContext>)builder, new SqlConnectionStringBuilder(connectionString));
            return builder;
        }

        private static void SetConnectionString<TContext>(FactoryBuilderBase<TContext> builder, SqlConnectionStringBuilder connStringBuilder)
            where TContext : DbContext
        {
            builder.GetConnectionString = () =>
            {
                var id = Guid.NewGuid().ToString().ToLower().Replace('-', '.');
                connStringBuilder.InitialCatalog = $"{typeof(TContext).Name}.{id}";
                return connStringBuilder.ConnectionString;
            };
        }
    }
}