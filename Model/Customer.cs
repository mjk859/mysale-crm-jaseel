using MySaleApp.Admin.UI.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class Customer : IEntityBase
    {
        public int Id { get; set; }

        public string? CustomerName { get; set; }
        public Guid CustomerGuid { get; set; }

        [Column(TypeName = "varchar(8)")]
        public string? CountryCode { get; set; }

        [Column(TypeName = "varchar(6)")]
        public string? CountryDialCode { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string? MobileNo { get; set; }

        //[Required]
        [Column(TypeName = "varchar(15)")]
        public string? WhatsappNo { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string? MobileVerificationOtp { get; set; }

        public bool IsMobileVerified { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(16)")]
        public string? EmailVerificationOtp { get; set; }

        public bool IsEmailVerified { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? DbName { get; set; }

        public int Licenses { get; set; }
        public int AdditionalLicense { get; set; }
        public bool IsDbCreated { get; set; }
        public bool IsDbProcessCompleted { get; set; } = false; //after intialization of alll intial data
        public string? PlanId { get; set; }
        public DateTime PlanExpiryDate { get; set; }
        public string? TRN { get; set; }
        public string? CountryId { get; set; }
        public string? CurrencyId { get; set; }
        public string? StateId { get; set; }
        public string? Platform { get; set; }
        public bool IsExpired { get; set; } = false;
        public bool IsLocked { get; set; }//

        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(25)")]
        public string? ReferralCode { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? ReferredBy { get; set; }

        public DateTime CreatedDate { get; set; }

        // [Column(TypeName = "varchar(60)")]
        public string? CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        // [Column(TypeName = "varchar(60)")]
        public string? ModifiedBy { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //[NotMapped]
        //public string? LoaggedUserId { get; set; }
    }
}