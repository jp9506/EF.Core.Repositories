using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Demo.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Scaffolding:ConnectionString", "Data Source=(local);Initial Catalog=Demo.Db;Integrated Security=true");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trigger"));

                entity.Property(e => e.CreatedDateUtc)
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedDateUtc)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trigger"));

                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.CreatedDateUtc)
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedDateUtc)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(d => d.Supervisor).WithMany(p => p.SupervisedUsers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_SupervisorId_Users_Id");

                entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UsersRole",
                        r => r.HasOne<Role>().WithMany()
                            .HasForeignKey("RoleId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_User_Roles_RoleId_Roles_Id"),
                        l => l.HasOne<User>().WithMany()
                            .HasForeignKey("UserId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_User_Roles_UserId_Users_Id"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PK_User_Roles");
                            j.ToTable("Users_Roles");
                        });
            });
        }
    }
}