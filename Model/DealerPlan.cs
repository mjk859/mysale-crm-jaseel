using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class DealerPlan : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CustomerDiscountPercentage { get; set; }
        public decimal DealerDiscountPercentage { get; set; }
        public bool IsCustomerDiscountEnabled { get; set; }
        public bool IsDealerDiscountEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}