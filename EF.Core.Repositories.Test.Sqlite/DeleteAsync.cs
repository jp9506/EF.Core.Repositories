using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sqlite.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Sqlite
{
    public class DeleteAsync
    {
        private const int USER_COUNT = 21;
        private readonly IRepositoryFactory<TestContext> _repositoryFactory;

        public DeleteAsync(IRepositoryFactory<TestContext> repositoryFactory, IDbContextFactory<TestContext> contextFactory)
        {
            _repositoryFactory = repositoryFactory;
            using var context = contextFactory.CreateDbContext();
            if (context.Database.EnsureCreated())
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
                context.SaveChanges();
            }
        }

        [Fact]
        public async void DeleteByIdAsync()
        {
            var user = new User
            {
                Email = "test@test.com",
                Id = new Guid("12345678-1234-1234-1234-1234567890AB"),
                Name = "Test Test",
                SupervisorId = null,
            };
            var repo = _repositoryFactory.GetRepository<User>();

            var res = await repo.DeleteByIdAsync(new { Id = user.Id });

            Assert.True(res);
            if (res)
            {
                var insertres = await repo.InsertAsync(user);
                var u = await repo.GetAsync(new { Id = user.Id });
                Assert.NotNull(u);
            }
        }

        [Fact]
        public async void DeleteEntityAsync()
        {
            var user = new User
            {
                Email = "test@test.com",
                Id = new Guid("12345678-1234-1234-1234-1234567890AB"),
                Name = "Test Test",
                SupervisorId = null,
            };
            var repo = _repositoryFactory.GetRepository<User>();

            var res = await repo.DeleteAsync(user);

            Assert.True(res);
            if (res)
                await repo.InsertAsync(user);
        }
    }
}