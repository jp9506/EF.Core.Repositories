using EF.Core.Repositories.Test.Sql.Data;
using System;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework("EF.Core.Repositories.Test.Sql.Startup", "EF.Core.Repositories.Test.Sql")]

namespace EF.Core.Repositories.Test.Sql
{
    public sealed class Startup : XunitTestFramework, IDisposable
    {
        public Startup(IMessageSink messageSink) : base(messageSink)
        {
            IFactoryBuilder<TestContext>.ConfigureContainerBuilder(builder =>
                builder
                    .WithLabel("Test", "Test")
                    .WithHostname("hostname12345")
            );
        }
    }
}