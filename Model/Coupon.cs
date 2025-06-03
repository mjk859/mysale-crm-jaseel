using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{ /// <summary>
  /// Represents a coupon that can be used for discounts.
  /// </summary>
    public class Coupon : IEntityBase

    {
        /// <summary>
        /// Gets or sets the unique identifier for the coupon.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique  guid identifier for the coupon.
        /// </summary>
        public Guid CouponId { get; set; }

        /// <summary>
        /// Gets or sets the Name of the coupon.
        /// </summary>
        public string CouponName { get; set; }

        /// <summary>
        /// Gets or sets the code of the coupon.
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the discount amount of the coupon.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount Percentage of the coupon.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        public bool IsDiscountPercentage { get; set; } = false;

        /// <summary>
        /// Gets or sets the start date of the coupon.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the expiration date of the coupon.
        /// </summary>

        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiry date whether its expire or not
        /// </summary>

        public bool IsNotExpire { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum  amount required to use the coupon.
        /// </summary>
        public decimal MinimumAmount { get; set; } = 0;

        /// <summary>
        /// Maximum Discount Allowed Per Redemption.
        /// </summary>
        public decimal MaximumDiscountAmount { get; set; } = 0;

        /// <summary>
        /// Multiple Redemption Allowed Per Customer.
        /// </summary>
        public bool IsMultipleRedemptionAllowed { get; set; } = false;

        /// <summary>
        /// if Applicable Only Plan Users
        /// </summary>
        public bool IsPlanOnlyCoupon { get; set; } = false;

        public string PlanId { get; set; } = "0"; //if planId="0" then applicable for all Plan other wise specified plan only

        /// <summary>
        /// if Applicable Only Additional License
        /// </summary>
        public bool IsAdditionalLicenseOnlyCoupon { get; set; } = false;

        /// <summary>
        /// if Applicable Only for Renewal Plan
        /// </summary>
        public bool IsRenewalPlanOnly { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the coupon is active.
        /// </summary>
        public bool IsActive { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ModifiedBy { get; set; }
    }
}