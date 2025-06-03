using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class CustomerCouponMapping : IEntityBase
    {
        public int Id { get; set; }

       //  public string CustomerId { get; set; }

        public string CouponId { get; set; }

        public string CustomerName { get; set; }

        public string CouponName { get; set; }

        public DateTime RedemptionDate { get; set; }

        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ModifiedBy { get; set; }
    }
}