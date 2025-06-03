using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class Plan : IEntityBase
    {
        public int Id { get; set; }
        public Guid PlanGuid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LocalName { get; set; }
        public decimal MonthlyPrice { get; set; } = 0;//plan charge eg 120/month
        public decimal AnnualPrice { get; set; } = 0;//plan charge eg 1200/month
        public decimal LicenseCharge { get; set; } = 0;//addtional License charge;/600/month

        public decimal DiscountPercentage { get; set; } = 0;//discount for anual charge
        public decimal LicenseDiscountPercentage { get; set; } = 0;//discount for license charge
        public decimal ReferralDiscountPercentage { get; set; } = 0;//discount for reffreal
        public bool IsTaxEnabled { get; set; } = true;
        public decimal TaxPercentage { get; set; } = 0;

        public bool IsBilledMonthly { get; set; } = false;
        public bool IsBilledAnually { get; set; } = false;
        public decimal LifeTimePrice { get; set; } = 0;
        public bool IsCustomPlan { get; set; }
        public bool IsLifeTimePlan { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsTrial { get; set; }
        public int TrialDays { get; set; }
        public bool IsOfferActivated { get; set; }
        public decimal OfferPercentage { get; set; }
        public DateTime OfferStart { get; set; }
        public DateTime OfferEnd { get; set; }
        public int CompaniesAllowed { get; set; } = 1;
        public int BranchAllowed { get; set; } = 1;
        public int UsersAllowed { get; set; } = 1;//User license allowed inthis plan,0= unlimited;
        public int InvoiceAllowed { get; set; } = 0;//0=unlimimted

        public decimal AdditionalInvoiceCharge { get; set; } = 0;
        public int VoucherEntryAllowed { get; set; } = 0;//0=unlimimted
        public decimal AdditionalVoucherEntryCharges { get; set; } = 0;

        public decimal TotalRevenueMonthly { get; set; } = 0;//0=unlimimted
        public decimal TotalRevenueYealry { get; set; } = 0;//0=unlimimted
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }
        public string? LogoPath { get; set; }
        public string? CountryCode { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string? CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string? ModifiedBy { get; set; }
    }
}