using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Extensions
{
    internal static class DbContextExtensions
    {
        public static async Task DisableForeignKeyContraintsAsync(this DbContext context, CancellationToken cancellationToken = default) => await SetForeignKeyConstraintsAsync(context, false, cancellationToken);

        public static async Task DisableIdentityInsertAsync(this DbContext context, IEntityType entityType, CancellationToken cancellationToken = default) => await SetIdentityInsertAsync(entityType, context, false, cancellationToken);

        public static async Task EnableForeignKeyContraintsAsync(this DbContext context, CancellationToken cancellationToken = default) => await SetForeignKeyConstraintsAsync(context, true, cancellationToken);

        public static async Task EnableIdentityInsertAsync(this DbContext context, IEntityType entityType, CancellationToken cancellationToken = default) => await SetIdentityInsertAsync(entityType, context, true, cancellationToken);

        public static async Task InsertEntityAsync(this DbContext context, EntityEntry entity, CancellationToken cancellationToken = default)
        {
            var parameters =
                entity.Properties
                .Where(p => p.CurrentValue != null)
                .Select((p, i) => new
                {
                    Column = p.Metadata.GetColumnName(),
                    Index = i,
                    Value = p.CurrentValue!,
                })
                .OrderBy(x => x.Index)
                .ToArray();
            var sql = $"INSERT INTO {entity.Metadata.GetSchemaQualifiedTableName()} " +
                $"({string.Join(',', parameters.Select(p => p.Column))}) " +
                $"VALUES ({string.Join(',', parameters.Select(p => $"@p{p.Index}"))})";
            await context.Database.ExecuteSqlRawAsync(
                sql,
                parameters.Select(p => p.Value),
                cancellationToken);
        }

        private static async Task SetForeignKeyConstraintsAsync(DbContext context, bool enable, CancellationToken cancellationToken)
        {
            var value = enable ? "CHECK" : "NOCHECK";
            await context.Database.ExecuteSqlRawAsync($"EXEC sp_MSforeachtable \"ALTER TABLE ? {value} CONSTRAINT ALL\"", cancellationToken);
        }

        private static async Task SetIdentityInsertAsync(IEntityType entityType, DbContext context, bool enable, CancellationToken cancellationToken)
        {
            var value = enable ? "ON" : "OFF";
            var hasIdentity = entityType
                .FindPrimaryKey()?
                .Properties
                .Any(x => x.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn) ?? false;
            if (hasIdentity)
                await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchemaQualifiedTableName()} {value}", cancellationToken);
        }
    }
}