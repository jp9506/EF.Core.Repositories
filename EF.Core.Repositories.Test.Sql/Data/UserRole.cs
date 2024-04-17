using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Repositories.Test.Sql.Data
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public DateTime? Expiration { get; set; }

        public virtual Role Role { get; set; } = null!;

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}