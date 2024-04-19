using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sql.Data;
using System;
using System.Linq;

namespace EF.Core.Repositories.Test.Sql
{
    public class GetAsync
    {
        private const string NULL_USER_ID = "EEF5FF63-0E05-4B9B-99E5-4D750353FD80";
        private const string USER_ID = "12345678-1234-1234-1234-1234567890AB";
        private readonly IFactoryBuilder<TestContext> _builder;

        private readonly string[] Ids = new[]
        {
            "295F2951-72A1-4F56-BA4B-00B7E19B1C44",
            "DE4894DD-54AC-4F5E-9FD6-5FA7A43EC83A",
            "5D9943CF-7C76-4EAE-B62F-DDAED3BBBA39",
            "4C132910-5910-41FB-A5A0-40ED3BA34A36",
            "F60867AC-D9E4-4433-9C2F-E0F2BD2F9254",
            "9C1AF471-8060-4B67-94BB-151C40FA9693",
            "C603FFDD-C541-4812-A635-DE8D7A615299",
            "5B485E27-253B-41CA-822F-777EFAF617A2",
            "1BF3DA93-B112-43A3-B496-66B7DC82C790",
            "C4B89C01-1C1F-41DA-8BC3-4448A6595C54",
            "F8D841CC-1E53-42BD-8689-52FCF9456F2F",
            "FAAB60A1-7F22-4E63-AF33-7648C69E860E",
            "64CF77ED-4777-4D53-8711-37F7E00D018E",
            "FC9E6470-6A57-4EFA-8CC6-1B41947D5D32",
            "4F9456F7-3576-4A0E-9E61-0DF419E4C868",
            "FA7B3258-1F2C-4655-AC88-40A9A8ACEB33",
            "B325155D-1F0D-4407-8919-6BBDD56718C6",
            "D71427EB-D4A1-4661-B256-ADE9239290E2",
            "AF05F64D-AE7A-499C-983D-B6DA5FD6251C",
            "06396C5E-CC86-4E47-B150-A3AC8A44810F",
            "467981A4-9815-4562-8B9C-9F987D23514D",
            "650DAD7C-97BD-4231-A37E-2F7853B0C142",
            "7B6BACCE-B007-4B11-B898-6BB8851FD520",
            "C6071F64-2C50-4301-8518-45436882EE9A",
            "AD6E61DA-8D14-4A60-9B7A-477AED518C5A",
            "DEE57457-2D0C-49B1-B9D6-89E5328FCF7C",
            "CEFCA142-2691-45DC-B85D-E7A557FA1347",
            "B34679EA-2424-4AE5-A258-EBAF32978439",
            "95135BBC-1186-4DB0-9974-B5C03D0E1770",
            "EB74F654-4651-4602-B5BF-FEE96A2AB6AB",
            "C79D7095-A48B-461D-9BFE-156796953A89",
            "B53DD328-565D-4A8E-BB5E-4802D8965530",
            "95037248-14B3-4F5B-A8D4-81F04E85FF1A",
            "1B029A01-11FC-4D97-BC2E-538666E9B67D",
            "BF692659-E674-4F46-A82C-A9CC1CAD16F6",
            "01BB18BF-B502-4031-AA04-6AEC3A582DFA",
            "8D0E6662-B00C-46AB-A028-FB10F1978633",
            "9A9BABF3-4922-43F3-9B58-328EA8118E2B",
            "AE53DA98-22BB-4919-A8FB-A5EA4A4C6641",
            "266E2539-8147-4927-AB59-C58F47AB6B47",
            "30E5145F-4E10-48BD-8518-F2E2D675F051",
            "DA709497-D6E3-43DA-BEA2-1FE825892CB7",
            "36EA0A6A-14D3-4DE1-B65C-659AAA4F173B",
            "83D40F31-634A-4A88-BDB8-0FACDECB920F",
            "443C476D-A850-4AAC-B077-8AC5981BA067",
            "A16178F2-B3F0-4F56-BEBB-F37D2BB9754B",
            "BE4AF0CA-03FE-4C42-83F6-C0C4A724C179",
        };

        public GetAsync()
        {
            _builder = IFactoryBuilder<TestContext>.Sql(Constants.CONNECTION_STRING,
                () =>
                    Ids.Select((x, i) => new User
                    {
                        Email = $"user-{i}@test.com",
                        Id = new Guid(x),
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
        public async void GetAllUsersAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var users = await repo.GetAsync();

            Assert.NotNull(users);
            Assert.Equal(Ids.Length + 1, users.Count());
        }

        [Fact]
        public async void GetUserByIdAsync()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var user = await repo.GetAsync(new { Id = new Guid(USER_ID) });

            Assert.NotNull(user);
            Assert.Equal(new Guid(USER_ID), user.Id);
            Assert.Equal("test@test.com", user.Email);
            Assert.Equal("Test Test", user.Name);
            Assert.Null(user.SupervisorId);
        }

        [Fact]
        public async void GetUserByIdAsync_NotFound()
        {
            using var factory = await _builder.CreateFactoryAsync();

            var repo = factory.GetRepository<User>();

            var user = await repo.GetAsync(new { Id = new Guid(NULL_USER_ID) });

            Assert.Null(user);
        }
    }
}