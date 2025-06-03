using System;
using System.ComponentModel.DataAnnotations.Schema;
using MySaleApp.Admin.UI.Interface;

namespace MySaleApp.Admin.UI.Models
{
    public class SystemUser : IEntityBase
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string UserRoleGuid { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsSystemAccount { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }

        [Column(TypeName = "varchar(90)")]
        public string LastIpAddress { get; set; }

        public DateTime LastLoginDateUtc { get; set; }

        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}