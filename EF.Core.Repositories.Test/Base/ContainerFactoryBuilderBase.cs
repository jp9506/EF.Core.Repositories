using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Base
{
    internal abstract class ContainerFactoryBuilderBase<TContext, TBuilder, THost, TConfiguration> : FactoryBuilderBase<TContext>
        where TContext : DbContext
        where TBuilder : ContainerBuilder<TBuilder, THost, TConfiguration>, new()
        where THost : DockerContainer, IContainer
        where TConfiguration : IContainerConfiguration
    {
        private static readonly object _hostLock = new();

        private static Func<TBuilder, TBuilder>? ConfigureBuilderFunc { get; set; }
        private static THost? Host { get; set; }

        internal static void SetConfigureBuilderFunc(Func<TBuilder, TBuilder> configureBuilderFunc)
        {
            lock (_hostLock)
            {
                ConfigureBuilderFunc = configureBuilderFunc;
            }
        }

        protected static THost GetHost()
        {
            EnsureHostStarted();
            return Host!;
        }

        private static void EnsureHostStarted()
        {
            lock (_hostLock)
            {
                if (Host is null)
                {
                    Host = GetBuilder().Build();
                    Host.StartAsync().Wait();
                }
            }
        }

        private static TBuilder GetBuilder()
        {
            var builder = new TBuilder();
            builder = ConfigureBuilderFunc?.Invoke(builder) ?? builder;
            return builder;
        }
    }
}