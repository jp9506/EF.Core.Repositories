using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Test.Extensions
{
    internal static class DbContextExtensions
    {
        public static async Task SeedDataAsync(this DbContext context, IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            await context.AddRangeAsync(entities, cancellationToken);
            var data = context.ChangeTracker.Entries().GroupBy(x => x.Metadata);
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            await context.DisableForeignKeyContraintsAsync(cancellationToken);
            foreach (var set in data)
            {
                await context.EnableIdentityInsertAsync(set.Key, cancellationToken);
                foreach (var e in set)
                {
                    await context.InsertEntityAsync(e, cancellationToken);
                }
                await context.DisableIdentityInsertAsync(set.Key, cancellationToken);
            }
            await context.EnableForeignKeyContraintsAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            foreach (var e in context.ChangeTracker.Entries())
            {
                e.State = EntityState.Unchanged;
            }
        }

        private static async Task DisableForeignKeyContraintsAsync(this DbContext context, CancellationToken cancellationToken = default) => await SetForeignKeyConstraintsAsync(context, false, cancellationToken);

        private static async Task DisableIdentityInsertAsync(this DbContext context, IEntityType entityType, CancellationToken cancellationToken = default) => await SetIdentityInsertAsync(entityType, context, false, cancellationToken);

        private static async Task EnableForeignKeyContraintsAsync(this DbContext context, CancellationToken cancellationToken = default) => await SetForeignKeyConstraintsAsync(context, true, cancellationToken);

        private static async Task EnableIdentityInsertAsync(this DbContext context, IEntityType entityType, CancellationToken cancellationToken = default) => await SetIdentityInsertAsync(entityType, context, true, cancellationToken);

        private static async Task InsertEntityAsync(this DbContext context, EntityEntry entity, CancellationToken cancellationToken = default)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "Script is safe from injection")]
        private static async Task SetForeignKeyConstraintsAsync(DbContext context, bool enable, CancellationToken cancellationToken)
        {
            var value = enable ? "CHECK" : "NOCHECK";
            await context.Database.ExecuteSqlRawAsync($"EXEC sp_MSforeachtable \"ALTER TABLE ? {value} CONSTRAINT ALL\"", cancellationToken);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "Script is safe from injection")]
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