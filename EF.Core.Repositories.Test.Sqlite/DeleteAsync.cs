using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sqlite.Data;
using System;

namespace EF.Core.Repositories.Test.Sqlite
{
    public class DeleteAsync
    {
        private const int USER_COUNT = 21;
        private readonly IFactoryBuilder<TestContext> _builder;

        public DeleteAsync()
        {
            _builder = new SqliteFactoryBuilder<TestContext>(context =>
            {
                context.Users.Add(new User
                {
                    Email = "test@test.com",
                    Id = new Guid("12345678-1234-1234-1234-1234567890AB"),
                    Name = "Test Test",
                    SupervisorId = null,
                });
                for (int i = 1; i < USER_COUNT; i++)
                {
                    context.Users.Add(new User
                    {
                        Email = $"user-{i}@test.com",
                        Id = Guid.NewGuid(),
                        Name = $"User {i}",
                        SupervisorId = null,
                    });
                }
            });
        }

        [Fact]
        public async void DeleteByIdAsync()
        {
            var factory = await _builder.CreateFactoryAsync();

            var user = new User
            {
                Email = "test@test.com",
                Id = new Guid("12345678-1234-1234-1234-1234567890AB"),
                Name = "Test Test",
                SupervisorId = null,
            };
            var repo = factory.GetRepository<User>();

            var res = await repo.DeleteByIdAsync(new { user.Id });
            var u = await repo.GetAsync(new { user.Id });

            Assert.True(res);
            Assert.Null(u);
        }

        [Fact]
        public async void DeleteEntityAsync()
        {
            var factory = await _builder.CreateFactoryAsync();

            var user = new User
            {
                Email = "test@test.com",
                Id = new Guid("12345678-1234-1234-1234-1234567890AB"),
                Name = "Test Test",
                SupervisorId = null,
            };
            var repo = factory.GetRepository<User>();

            var res = await repo.DeleteAsync(user);
            var u = await repo.GetAsync(new { user.Id });

            Assert.True(res);
            Assert.Null(u);
        }
    }
}