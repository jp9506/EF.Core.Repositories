using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Data
{
    public partial class Role
    {
        [Column("CreatedDateUTC", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDateUtc { get; set; }

        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [Column("UpdatedDateUTC", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDateUtc { get; set; }

        /// [ForeignKey("RoleId")] [InverseProperty("Roles")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}