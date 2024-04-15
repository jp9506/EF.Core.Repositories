using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF.Core.Repositories.Test.Sqlite.Data
{
    public class User
    {
        public string? Email { get; set; }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public virtual ICollection<User> SupervisedUsers { get; set; } = new List<User>();
        public virtual User? Supervisor { get; set; }
        public Guid? SupervisorId { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}