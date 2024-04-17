using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Repositories.Test.Sql.Data
{
    public class Class
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}