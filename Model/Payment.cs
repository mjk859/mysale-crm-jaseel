using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class Payment : IEntityBase
    {
        public int Id { get; set; }
        public Guid PaymentGuid { get; set; }
        public string CustomerGuid { get; set; }
        public string PlanGuid { get; set; }
        public string SubscriptionGuid { get; set; }
        public int AddressId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyId { get; set; }
        public string CountryCode { get; set; }
        public string CountryId { get; set; }
        public int AdditionalLicense { get; set; } = 0;
        public decimal AdditionalLicenseCharge { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountP { get; set; }
        public decimal NetValue { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxP { get; set; }
        public decimal NetTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDescription { get; set; }
        public decimal TransactedAmount { get; set; } //Amount confirmation from payment gateway
        public string GateWayTransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool TransactionStatus { get; set; }
        public string StatusMessage { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionResponse { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ModifiedBy { get; set; }
    }
}