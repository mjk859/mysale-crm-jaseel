using System;

namespace MySaleApp.Admin.UI.ViewModel
{
    public class CouponViewModel
    {
        public string CouponName { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsDiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsNotExpire { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumDiscountAmount { get; set; }
        public bool IsMultipleRedemptionAllowed { get; set; }
        public bool IsPlanOnlyCoupon { get; set; }
        public string PlanId { get; set; }
        public bool IsAdditionalLicenseOnlyCoupon { get; set; }
        public bool IsRenewalPlanOnly { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }

    }
}
