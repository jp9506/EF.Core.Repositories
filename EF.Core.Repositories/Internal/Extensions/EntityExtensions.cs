using EF.Core.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EF.Core.Repositories.Internal.Extensions
{
    internal static class EntityExtensions
    {
        public static string? GetKeyHashCode<T>(this DbContext context, T entity)
            where T : class?
        {
            if (entity == null)
                return null;
            var keyValues = context.GetKeyValues(entity);
            return string.Join(',',
                keyValues!
                    .Select(v => v.Value?.ToString() ?? "null"));
        }

        public static IEnumerable<IProperty> GetKeyProperties<T>(this DbContext context)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            var keys = entityType?.FindPrimaryKey();
            return keys?.Properties ?? Enumerable.Empty<IProperty>();
        }

        private static Dictionary<string, object?>? GetKeyValues<T>(this DbContext context, T entity)
                    where T : class?
        {
            if (entity == null)
                return null;
            var keyProps = context.GetKeyProperties<T>();
            return keyProps
                .ToDictionary(
                    p => p.Name,
                    p => p.GetGetter().GetClrValue(entity));
        }
    }
}