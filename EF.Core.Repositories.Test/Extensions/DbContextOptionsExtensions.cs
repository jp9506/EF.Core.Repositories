using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Extensions
{
    internal static class DbContextOptionsExtensions
    {
        public static TContext NewDbContext<TContext>(this DbContextOptions<TContext> options)
            where TContext : DbContext
        {
            var ci = typeof(TContext).GetConstructor(new Type[] { typeof(DbContextOptions<TContext>) });
            if (ci?.Invoke(new[] { options }) is not TContext ctx)
            {
                throw new InvalidOperationException($"Type {typeof(TContext).FullName} does not have constructor accepting an argument of type {typeof(DbContextOptions<TContext>).FullName}.");
            }
            return ctx;
        }
    }
}