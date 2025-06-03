using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class Country : IEntityBase
    {
        public int Id { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryLocalName { get; set; }
        public string CountryCode { get; set; }
        public string TwoLetterISOcode { get; set; } = "0";
        public string ThreeLetterISOcode { get; set; } = "0";
        public int NumericISOcode { get; set; } = 0;
        public string CurrencyId { get; set; }
        public bool IsTaxEnabled { get; set; } = false;
        public decimal TaxP { get; set; } = 0;
        public bool IsCanceled { get; set; } = false;
        public bool Published { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
        public int DisplayOrder { get; set; }
    }
}