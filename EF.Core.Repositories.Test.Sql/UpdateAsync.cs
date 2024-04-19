using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;

namespace EF.Core.Repositories.Test.Sql
{
    public class UpdateAsync
    {
        private const string CLASS_ID = "B983BF3D-7FBB-4206-8FDF-9BDD9E329D6C";
        private const int MANAGER_ROLE_ID = 1;
        private const string SUPER_USER_ID = "19E9E0C9-B5A2-449A-A08E-53D2A6483223";
        private const string USER_ID = "12345678-1234-1234-1234-1234567890AB";
        private readonly IFactoryBuilder<TestContext> _builder;

        public UpdateAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Sql(Constants.CONNECTION_STRING,
                () => new object[]
                {
                    new Role
                    {
                        Id = MANAGER_ROLE_ID,
                        Name = "Manager",
                    },
                    new User
                    {
                        Email = "test@test.com",
                        Id = new Guid(USER_ID),
                        Name = "Test Test",
                        SupervisorId = null,
                        Classes = new[]
                        {
                            new Class
                            {
                                Id = new Guid(CLASS_ID),
                                Name = "Test Class",
                            },
                        }
                    },
                    new User
                    {
                        Email = "super@super.com",
                        Id = new Guid(SUPER_USER_ID),
                        Name = "Super Super",
                        SupervisorId = null,
                        UserRoles = new[]
                        {
                            new UserRole
                            {
                                Expiration = null,
                                RoleId = MANAGER_ROLE_ID,
                                UserId = new Guid(SUPER_USER_ID),
                            },
                        }
                    }
                });
        }

        [Fact]
        public async void UpdateUserAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var user = await repo.GetAsync(new { Id = new Guid(USER_ID) });

            Assert.NotNull(user);
            Assert.Equal(new Guid(USER_ID), user.Id);
            Assert.Equal("test@test.com", user.Email);
            Assert.Equal("Test Test", user.Name);
            Assert.Null(user.SupervisorId);

            user.SupervisorId = new Guid(SUPER_USER_ID);

            user = await repo.UpdateAsync(user);

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.SupervisorId);
        }

        [Fact]
        public async void UpdateUserIncludeClasses()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var classRepo = factory
                .GetRepository<Class>();

            var @class = await classRepo.GetAsync(new { Id = new Guid(CLASS_ID) });

            Assert.NotNull(@class);
            Assert.Equal(new Guid(CLASS_ID), @class.Id);

            var userRepo = factory
                .GetRepository<User>()
                .Include(x => x.Classes);

            var user = await userRepo.GetAsync(new { Id = new Guid(SUPER_USER_ID) });

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.Id);
            Assert.Empty(user.Classes);

            user.Classes.Add(@class);

            user = await userRepo.UpdateAsync(user);

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.Id);
            Assert.Single(user.Classes);

            user = await userRepo.GetAsync(new { Id = new Guid(SUPER_USER_ID) });

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.Id);
            Assert.Single(user.Classes);
        }

        [Fact]
        public async void UpdateUserIncludeRoles()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory
                .GetRepository<User>()
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role);

            var user = await repo.GetAsync(new { Id = new Guid(SUPER_USER_ID) });

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.Id);
            Assert.Single(user.UserRoles);

            var userRole = user.UserRoles.FirstOrDefault();

            Assert.NotNull(userRole);
            Assert.Null(userRole.Expiration);

            var exp = DateTime.UtcNow;

            userRole.Expiration = exp;

            user = await repo.UpdateAsync(user);

            Assert.NotNull(user);
            Assert.Equal(new Guid(SUPER_USER_ID), user.Id);
            Assert.Single(user.UserRoles);

            userRole = user.UserRoles.FirstOrDefault();

            Assert.NotNull(userRole);
            Assert.NotNull(userRole.Expiration);
            Assert.Equal(exp, userRole.Expiration);
        }
    }
}