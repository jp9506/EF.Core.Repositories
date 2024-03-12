using Azure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data
{
    public partial class User
    {
        [Column("CreatedDateUTC", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDateUtc { get; set; }

        [StringLength(50)]
        [Required]
        public string EmailAddress { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Key]
        public Guid Id { get; set; }

        public bool IsEnabled { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        /// [ForeignKey("UserId")] [InverseProperty("Users")]
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        /// [InverseProperty("Supervisor")]
        public virtual ICollection<User> SupervisedUsers { get; set; } = new List<User>();

        /// [ForeignKey("SupervisorId")] [InverseProperty("InverseSupervisor")]
        public virtual User Supervisor { get; set; } = null!;

        [Required]
        public Guid SupervisorId { get; set; }

        [Column("UpdatedDateUTC", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDateUtc { get; set; }

        [StringLength(50)]
        [Required]
        public string Username { get; set; } = null!;
    }
}