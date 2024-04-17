using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;

namespace EF.Core.Repositories.Test.Sql
{
    public class DeleteAsync
    {
        private const int USER_COUNT = 21;
        private const string USER_ID = "12345678-1234-1234-1234-1234567890AB";
        private readonly IFactoryBuilder<TestContext> _builder;

        public DeleteAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Sql(Constants.CONNECTION_STRING,
                () =>
                    Enumerable.Range(1, USER_COUNT - 1)
                        .Select(i => new User
                        {
                            Email = $"user-{i}@test.com",
                            Id = Guid.NewGuid(),
                            Name = $"User {i}",
                            SupervisorId = null,
                        })
                        .Union(new[]
                        {
                            new User
                            {
                                Email = "test@test.com",
                                Id = new Guid(USER_ID),
                                Name = "Test Test",
                                SupervisorId = null,
                            }
                        }));
        }

        [Fact]
        public async void DeleteUserAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var user = new User
            {
                Email = "test@test.com",
                Id = new Guid(USER_ID),
                Name = "Test Test",
                SupervisorId = null,
            };
            var repo = factory.GetRepository<User>();

            var res = await repo.DeleteAsync(user);
            var u = await repo.GetAsync(new { user.Id });

            Assert.True(res);
            Assert.Null(u);
        }

        [Fact]
        public async void DeleteUserByIdAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var res = await repo.DeleteByIdAsync(new { Id = new Guid(USER_ID) });
            var u = await repo.GetAsync(new { Id = new Guid(USER_ID) });

            Assert.True(res);
            Assert.Null(u);
        }
    }
}