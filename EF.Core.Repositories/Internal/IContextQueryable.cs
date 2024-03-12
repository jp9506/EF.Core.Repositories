using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Internal
{
    internal interface IContextQueryable<out T> : IQueryable<T>
    {
        DbContext Context { get; }
    }
}