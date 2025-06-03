using MySaleApp.Admin.UI.Interface;

using System;

namespace MySaleApp.Admin.UI.Models
{
    public class Address : IEntityBase
    {
        public int Id { get; set; }
       // public string CustomerId { get; set; }

        public string AddressType { get; set; }//Billing or Shipping

        public string Name { get; set; }//Name Optional

        public string LocalName { get; set; }//Name Optional

        public string Address0 { get; set; }

        public string LocalAddress { get; set; }

        public string Address1 { get; set; }

        public string MobileNo { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string ZipCode { get; set; }

        public string CountryId { get; set; }

        public string StateId { get; set; }

        public bool IsPrimary { get; set; } = false;

        public bool Status { get; set; } = true;

        public bool DisplayOrder { get; set; }

        public bool IsCanceled { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}