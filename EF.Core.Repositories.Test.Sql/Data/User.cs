using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Repositories.Test.Sql.Data
{
    public class User
    {
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public string? Email { get; set; }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public virtual ICollection<User> SupervisedUsers { get; set; } = new List<User>();
        public virtual User? Supervisor { get; set; }

        [ForeignKey(nameof(Supervisor))]
        public Guid? SupervisorId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}