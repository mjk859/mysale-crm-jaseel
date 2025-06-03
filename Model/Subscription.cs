using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class Subscription : IEntityBase
    {
        public int Id { get; set; }
        public Guid SubscriptionId { get; set; }
        public String CustomerGuid { get; set; }
        public string CustomerPlanGuid { get; set; }
        public string PaymentGuid { get; set; }
        public bool IsBilledMonthly { get; set; } = false;
        public decimal MonthlyPrice { get; set; } = 0;//plan charge eg 120/month
        public bool IsBilledAnually { get; set; } = false;
        public decimal AnnualPrice { get; set; } = 0;//plan charge eg 1200/month
        public decimal LicenseCharge { get; set; } = 0;
        public bool IsTaxEnabled { get; set; } = true;
        public decimal TaxP { get; set; } = 0;
        public int CompaniesAllowed { get; set; } = 1;
        public int BranchAllowed { get; set; } = 1;
        public int UsersAllowed { get; set; } = 1;//User license allowed inthIs plan,0= unlimited;
        public int InvoiceAllowed { get; set; } = 0;//0=unlimimted
        public decimal AdditionalInvoiceCharge { get; set; } = 0;
        public int VoucherEntryAllowed { get; set; } = 0;//0=unlimimted
        public decimal AdditionalVoucherEntryCharges { get; set; } = 0;
        public decimal TotalRevenueMonthly { get; set; } = 0;//0=unlimimted
        public decimal TotalRevenueYealry { get; set; } = 0;//0=unlimimted
        public int TrialDays { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsLifeTimePlan { get; set; } = false;
        public string RefferalCode { get; set; }
        public bool Status { get; set; } = true;
        public bool Active { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public bool IsCurrentSubscription { get; set; } = false;
        public string CurrencyId { get; set; }
        public string CountryId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}