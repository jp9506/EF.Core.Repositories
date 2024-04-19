using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EF.Core.Repositories.Test.Sql.Data
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasMany(d => d.Users).WithMany(p => p.Classes)
                    .UsingEntity<Dictionary<string, object>>(
                        "ClassesUser",
                        r => r.HasOne<User>().WithMany()
                            .HasForeignKey("UserId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Classes_Users_UserId_Users_Id"),
                        l => l.HasOne<Class>().WithMany()
                            .HasForeignKey("ClassId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Classes_Users_ClassId_Classes_Id"),
                        j =>
                        {
                            j.HasKey("ClassId", "UserId");
                            j.ToTable("Classes_Users");
                        });
            });
            modelBuilder.Entity<Role>(entity =>
            {
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(x => x.Role).WithMany(p => p.UserRoles)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserRoles_RoleId_Roles_Id");
                entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserRoles_UserId_Users_Id");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.HasOne(x => x.Supervisor).WithMany(p => p.SupervisedUsers)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Users_SupervisorId_User_Id");
            });
        }
    }
}