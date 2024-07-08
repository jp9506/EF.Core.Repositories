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
        private readonly object _hostLock = new();

        internal Func<TBuilder, TBuilder>? ConfigureBuilderFunc { get; set; }
        private THost? Host { get; set; }

        protected THost GetHost()
        {
            EnsureHostStarted();
            return Host!;
        }

        private void EnsureHostStarted()
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

        private TBuilder GetBuilder()
        {
            var builder = new TBuilder();
            builder = ConfigureBuilderFunc?.Invoke(builder) ?? builder;
            return builder;
        }
    }
}