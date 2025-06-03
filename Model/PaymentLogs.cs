using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class PaymentLogs : IEntityBase
    {
        public int Id { get; set; }
        public Guid PaymentGuid { get; set; }
        public Guid CustomerGuid { get; set; }
        public Guid PlanGuid { get; set; }
        public string CurrencyId { get; set; }
        public string CountryId { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountP { get; set; }
        public decimal NetValue { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatP { get; set; }
        public decimal NetTotal { get; set; }
        public String PaymentMethod { get; set; }
        public String TransactionId { get; set; }
        public String GateWayTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool TransactionStatus { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ModifiedBy { get; set; }
    }
}