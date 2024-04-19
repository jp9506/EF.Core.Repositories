using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;

namespace EF.Core.Repositories.Test.Sql
{
    public class InsertAsync
    {
        private const string SUPER_USER_ID = "19E9E0C9-B5A2-449A-A08E-53D2A6483223";
        private const string USER_ID = "12345678-1234-1234-1234-1234567890AB";
        private readonly IFactoryBuilder<TestContext> _builder;

        public InsertAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Sql(Constants.CONNECTION_STRING,
                () => new[]
                {
                    new User
                    {
                        Email = "super@super.com",
                        Id = new Guid(SUPER_USER_ID),
                        Name = "Super Super",
                        SupervisorId = null,
                    },
                });
        }

        [Fact]
        public async void InsertUserAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var user = new User
            {
                Email = "test@test.com",
                Name = "Test Test",
                SupervisorId = new Guid(SUPER_USER_ID),
            };

            user = await repo.InsertAsync(user);

            Assert.NotNull(user);
            Assert.Equal("test@test.com", user.Email);
            Assert.Equal("Test Test", user.Name);
            Assert.Equal(new Guid(SUPER_USER_ID), user.SupervisorId);
        }
    }
}