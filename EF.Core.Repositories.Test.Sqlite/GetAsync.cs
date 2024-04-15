using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sqlite.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EF.Core.Repositories.Test.Sqlite
{
    public class GetAsync
    {
        private const int USER_COUNT = 21;
        private readonly IRepositoryFactory<TestContext> _repositoryFactory;

        public GetAsync(IRepositoryFactory<TestContext> repositoryFactory, IDbContextFactory<TestContext> contextFactory)
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
        public async void GetAllAsync()
        {
            var repo = _repositoryFactory.GetRepository<User>();

            var users = await repo.GetAsync();

            Assert.NotNull(users);
            Assert.Equal(USER_COUNT, users.Count());
        }

        [Fact]
        public async void GetByIdAsync()
        {
            var repo = _repositoryFactory.GetRepository<User>();

            var user = await repo.GetAsync(new { Id = new Guid("12345678-1234-1234-1234-1234567890AB") });

            Assert.NotNull(user);
            Assert.Equal(new Guid("12345678-1234-1234-1234-1234567890AB"), user.Id);
            Assert.Equal("test@test.com", user.Email);
            Assert.Equal("Test Test", user.Name);
            Assert.Null(user.SupervisorId);
        }
    }
}