using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF.Core.Repositories.Test.Sql.Data
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}