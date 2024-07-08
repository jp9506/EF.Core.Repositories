using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sql
{
    public class AnyAllAsync
    {
        private const int COUNT = 1000;
        private readonly IFactoryBuilder<TestContext> _builder;

        public AnyAllAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Instance()
                //.WithConnectionString(Constants.CONNECTION_STRING)
                .WithSeed(() =>
                    Enumerable.Range(1, COUNT)
                    .Select(i => new Class
                    {
                        Cost = 50 * i,
                        Id = Guid.NewGuid(),
                        Name = $"Class {i}",
                    }));
        }

        [Fact]
        public async Task AllClassesNotFreeAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AllAsync(x => x.Cost > 0);

            Assert.True(b);
        }

        [Fact]
        public async Task AllClassesOver50Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AllAsync(x => x.Cost > 50);

            Assert.False(b);
        }

        [Fact]
        public async Task AnyClassesAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync();

            Assert.True(b);
        }

        [Fact]
        public async Task AnyClassesEqual49Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync(x => x.Cost == 49);

            Assert.False(b);
        }

        [Fact]
        public async Task AnyClassesOver5000Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync(x => x.Cost > 5000);

            Assert.True(b);
        }

        [Fact]
        public async Task AnyUsersAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var b = await repo.AnyAsync();

            Assert.False(b);
        }
    }
}