using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        public string? ParentCustomerId { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string? UserName { get; set; }

        [Column(TypeName = "varchar(6)")]
        public string? CountryDialCode { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? MobileNo { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string? MobileVerificationOtp { get; set; }

        public bool IsMobileVerified { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(16)")]
        public string? EmailVerificationOtp { get; set; }

        public bool IsEmailVerified { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? Password { get; set; }

        public string? PasswordSalt { get; set; }
        public bool IsPasswordCreated { get; set; }

        public bool IsWebUser { get; set; } = false;//For DashbOard mysale.cloud

        public bool IsAppUser { get; set; } = false;//for app
        public bool IsBooksUser { get; set; } = false;//for mysalebooks

        public bool IsSupervisor { get; set; } = false;//He Should accses to report only to view and export
        public bool IsActive { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsSystemAccount { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }

        [Column(TypeName = "varchar(90)")]
        public string? LastIpAddress { get; set; }

        public DateTime LastLoginDateUtc { get; set; }
        public string? Platform { get; set; }
        public string? BuildVersion { get; set; }
        // public virtual Customer Customer { get; set; }

        [NotMapped]
        public string? DbName { get; set; }

        public string? UserRoleGuid { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string? CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string? ModifiedBy { get; set; }
    }
}