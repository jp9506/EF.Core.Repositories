using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using EF.Core.Repositories.Test.Base;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Extensions
{
    /// <summary>
    /// Extensions for configuring containers for a <see cref="IFactoryBuilder{TContext}"/>
    /// </summary>
    public static partial class IFactoryBuilderContainerExtensions
    {
        private static void SetConfigureBuilderFunc<TContext, TBuilder, THost, TConfiguration>(ContainerFactoryBuilderBase<TContext, TBuilder, THost, TConfiguration> builder, Func<TBuilder, TBuilder> configureBuilderFunction)
            where TContext : DbContext
            where TBuilder : ContainerBuilder<TBuilder, THost, TConfiguration>, new()
            where THost : DockerContainer, IContainer
            where TConfiguration : IContainerConfiguration

        {
            builder.ConfigureBuilderFunc = configureBuilderFunction;
        }
    }
}