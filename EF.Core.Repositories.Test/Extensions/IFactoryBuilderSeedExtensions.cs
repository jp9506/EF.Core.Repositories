using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Extensions
{
    /// <summary>
    /// Extensions for specifying seed data to a <see cref="IFactoryBuilder{TContext}"/>
    /// </summary>
    public static class IFactoryBuilderSeedExtensions
    {
        /// <summary>
        /// Specifies seeding data for the <see cref="IFactoryBuilder{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> WithSeed<TContext>(this IFactoryBuilder<TContext> builder, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction)
            where TContext : DbContext
        {
            SetSeed((FactoryBuilderBase<TContext>)builder, seedFunction);
            return builder;
        }

        /// <summary>
        /// Specifies seeding data for the <see cref="IFactoryBuilder{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> WithSeed<TContext>(this IFactoryBuilder<TContext> builder, Func<IEnumerable<object>> seedFunction)
            where TContext : DbContext
        {
            SetSeed((FactoryBuilderBase<TContext>)builder, async token => await Task.Run(() => seedFunction(), token));
            return builder;
        }

        /// <summary>
        /// Specifies seeding data for the <see cref="IFactoryBuilder{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of <see cref="DbContext"/> for the factory.</typeparam>
        /// <param name="builder">The <see cref="IFactoryBuilder{TContext}"/>.</param>
        /// <param name="seedFunction">A function to provide entities to the backing data structure.</param>
        /// <returns>The <see cref="IFactoryBuilder{TContext}"/>.</returns>
        public static IFactoryBuilder<TContext> WithSeed<TContext>(this IFactoryBuilder<TContext> builder, Func<Task<IEnumerable<object>>> seedFunction)
            where TContext : DbContext
        {
            SetSeed((FactoryBuilderBase<TContext>)builder, async _ => await seedFunction());
            return builder;
        }

        private static void SetSeed<TContext>(FactoryBuilderBase<TContext> builder, Func<CancellationToken, Task<IEnumerable<object>>> seedFunction)
            where TContext : DbContext
        {
            builder.GetSeed = seedFunction;
        }
    }
}