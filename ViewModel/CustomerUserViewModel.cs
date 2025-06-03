using System;

namespace MySaleApp.Admin.UI.ViewModel
{
    public class CustomerUserViewModel
    {

        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string CountryDialCode { get; set; }
        public string MobileNo { get; set; }

        public string MobileVerificationOtp { get; set; }
        public bool IsMobileVerified { get; set; }
        public string Email { get; set; }
        public string EmailVerificationOtp { get; set; }
        public bool IsEmailVerified { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsPasswordCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsSystemAccount { get; set; }
        public DateTime LastActivityDateUtc { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime LastLoginDateUtc { get; set; }

        public string DbName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        //public string UserRoleGuid { get; set; }
        //public string UserRoleName { get; set; }
        public string ParentCustomerId { get; set; }
        public string CustomerName { get; set; }

    }
}
