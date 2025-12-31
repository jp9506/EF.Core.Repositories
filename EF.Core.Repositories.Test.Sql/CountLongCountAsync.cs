using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Sql
{
    public class CountLongCountAsync
    {
        private const int COUNT = 1000;
        private readonly IFactoryBuilder<TestContext> _builder;

        public CountLongCountAsync()
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
        public async Task AllClassesCost50CountAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.CountAsync(x => x.Cost == 50);

            Assert.Equal(1, b);
        }

        [Fact]
        public async Task AllClassesCost50LongCountAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.LongCountAsync(x => x.Cost == 50);

            Assert.Equal(1, b);
        }

        [Fact]
        public async Task AllClassesCountAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.CountAsync();

            Assert.Equal(COUNT, b);
        }

        [Fact]
        public async Task AllClassesLongCountAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<Class>();

            var b = await repo.LongCountAsync();

            Assert.Equal(COUNT, b);
        }
    }
}