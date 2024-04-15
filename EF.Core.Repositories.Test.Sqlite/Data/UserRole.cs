using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Repositories.Test.Sqlite.Data
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public DateTime? Expiration { get; set; }

        public virtual Role Role { get; set; } = null!;

        public int RoleId { get; set; }

        public virtual User User { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}