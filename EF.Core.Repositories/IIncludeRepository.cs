using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories
{
    /// <summary>
    /// Supports <see cref="IRepository{T}"/> Include/ThenInclude chaining operators.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TProp">The property type.</typeparam>
    public interface IIncludeRepository<TEntity, TProp> : IRepository<TEntity>
        where TEntity : class
        where TProp : class?
    {
    }

    internal interface IExtensibleIncludeCollectionRepository<TEntity, TProp>
        where TEntity : class
        where TProp : class?
    {
        IIncludableQueryable<TEntity, ICollection<TProp>> EntityQuery(DbContext context);
    }

    internal interface IExtensibleIncludeRepository<TEntity, TProp>
        where TEntity : class
        where TProp : class?
    {
        IIncludableQueryable<TEntity, TProp> EntityQuery(DbContext context);
    }

    internal interface IInternalIncludeRepository<TEntity, TProp> : IIncludeRepository<TEntity, TProp>, IInternalRepository<TEntity>
        where TEntity : class
        where TProp : class?
    {
        Task HandleExpressionUpdateAsync(DbContext context, TEntity current, TEntity entity, Func<TProp, TProp, Task> then, CancellationToken cancellationToken = default);
    }
}