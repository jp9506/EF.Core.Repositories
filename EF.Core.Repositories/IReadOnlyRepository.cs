﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EF.Core.Repositories
{
    /// <summary>
    /// Provides readonly functionality to a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of data in the repository.</typeparam>
    public interface IReadOnlyRepository<T>
    {
    }

    internal interface IInternalReadOnlyRepository<T> : IReadOnlyRepository<T>
    {
        IInternalTransaction Transaction { get; }

        IQueryable<T> EntityQuery(DbContext context);
    }
}