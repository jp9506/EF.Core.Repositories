using EF.Core.Repositories.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EF.Core.Repositories.Extensions
{
    /// <summary>
    /// Extension methods for registering <see cref="IRepositoryFactory{TContext}"/> within the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers an <see cref="IRepositoryFactory{TContext}"/> in the <see
        /// cref="IServiceCollection"/> to create instances of <see cref="IRepository{T}"/> and <see
        /// cref="IReadOnlyRepository{T}"/> using a <see cref="DbContext"/> of type <typeparamref name="TContext"/>.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of <see cref="DbContext"/> to be created by the factory.
        /// </typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="optionsAction">
        /// <para>
        /// An optional action to configure the <see cref="DbContextOptions"/> for the context. This
        /// provides an alternative to performing configuration of the context by overriding the
        /// <see cref="DbContext.OnConfiguring"/> method in your derived context.
        /// </para>
        /// <para>
        /// If an action is supplied here, the <see cref="DbContext.OnConfiguring"/> method will
        /// still be run if it has been overridden on the derived context. <see
        /// cref="DbContext.OnConfiguring"/> configuration will be applied in addition to
        /// configuration performed here.
        /// </para>
        /// <para>
        /// In order for the options to be passed into your context, you need to expose a
        /// constructor on your context that takes <see cref="DbContextOptions{TContext}"/> and
        /// passes it to the base constructor of <see cref="DbContext"/>.
        /// </para>
        /// </param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection ConfigureData<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = default)
            where TContext : DbContext
        {
            return services
                .AddDbContextFactory<TContext>(optionsAction)
                .AddSingleton<IRepositoryFactory<TContext>, RepositoryFactory<TContext>>();
        }
    }
}