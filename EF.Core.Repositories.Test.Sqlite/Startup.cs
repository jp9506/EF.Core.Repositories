using EF.Core.Repositories.Extensions;
using EF.Core.Repositories.Test.Sqlite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Core.Repositories.Test.Sqlite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureData<TestContext>(opt => opt.UseSqlite("DataSource=file::memory:?cache=shared"));
        }
    }
}