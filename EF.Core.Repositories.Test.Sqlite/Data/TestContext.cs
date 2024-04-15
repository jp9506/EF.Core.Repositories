using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Test.Sqlite.Data
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Users_SupervisorId_User_Id");
            });
        }
    }
}