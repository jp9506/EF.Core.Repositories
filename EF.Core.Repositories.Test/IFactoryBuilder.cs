using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    public interface IFactoryBuilder<TContext>
        where TContext : DbContext
    {
        Task<IRepositoryFactory<TContext>> CreateFactoryAsync(CancellationToken cancellationToken = default);
    }
}