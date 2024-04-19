using EF.Core.Repositories.Test.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test
{
    internal abstract class RepositoryFactoryBase<TContext> : IRepositoryFactory<TContext>
        where TContext : DbContext
    {
        private bool disposedValue;
        protected DbContextOptions<TContext> _options { get; init; } = null!;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        async Task<DbContext> IRepositoryFactory.GetDbContextAsync(CancellationToken cancellationToken)
        {
            return await GetDbContextAsync(cancellationToken);
        }

        public async Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default) => await Task.Run(() =>
        {
            return _options.NewDbContext();
        }, cancellationToken);

        protected virtual void OnDisposing()
        { }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDisposing();
                }

                disposedValue = true;
            }
        }
    }
}