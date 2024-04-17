using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF.Core.Repositories.Test.Sql.Data
{
    public class Class
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}