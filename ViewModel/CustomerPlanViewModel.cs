using MySaleApp.Admin.UI.Models;
using System.Collections.Generic;
using System;

namespace MySaleApp.Admin.UI.ViewModel
{
    public class CustomerPlanViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerGuid { get; set; }
        public string CountryCode { get; set; }
        public string CountryDialCode { get; set; }
        public string MobileNo { get; set; }
        public string MobileVerificationOtp { get; set; }
        public bool IsMobileVerified { get; set; }
        public string Email { get; set; }
        public string EmailVerificationOtp { get; set; }
        public bool IsEmailVerified { get; set; }
        public string DbName { get; set; }
        public int Licenses { get; set; }
        public string PlanId { get; set; }
        public bool IsDbCreated { get; set; }
        public bool IsDbProcessCompleted { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string ReferralCode { get; set; }
        public string ReferredBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public string PlanName { get; set; }
        public DateTime OfferEndDate { get; set; }
        public DateTime? PlanExpiryDate { get; set; }
        public string Platform { get; set; }
        public string PaymentId { get; set; }
        public List<User> Users { get; set; }

        // public List<Subscription> Subscriptions { get; set; }
        public List<Payment> Payment { get; set; }
        public object OfferEnd { get; internal set; }
        public int AdditionalLicense { get; internal set; }
    }
}