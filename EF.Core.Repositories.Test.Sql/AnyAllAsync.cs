using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;

namespace EF.Core.Repositories.Test.Sql
{
    public class AnyAllAsync
    {
        private const int COUNT = 1000;
        private readonly IFactoryBuilder<TestContext> _builder;

        public AnyAllAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Sql(Constants.CONNECTION_STRING,
                () =>
                    Enumerable.Range(1, COUNT)
                    .Select(i => new Class
                    {
                        Cost = 50 * i,
                        Id = Guid.NewGuid(),
                        Name = $"Class {i}",
                    }));
        }

        [Fact]
        public async void AllClassesNotFreeAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AllAsync(x => x.Cost > 0);

            Assert.True(b);
        }

        [Fact]
        public async void AllClassesOver50Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AllAsync(x => x.Cost > 50);

            Assert.False(b);
        }

        [Fact]
        public async void AnyClassesAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync();

            Assert.True(b);
        }

        [Fact]
        public async void AnyClassesEqual49Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync(x => x.Cost == 49);

            Assert.False(b);
        }

        [Fact]
        public async void AnyClassesOver5000Async()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.AnyAsync(x => x.Cost > 5000);

            Assert.True(b);
        }

        [Fact]
        public async void AnyUsersAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var b = await repo.AnyAsync();

            Assert.False(b);
        }
    }
}