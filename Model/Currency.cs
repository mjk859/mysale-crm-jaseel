using MySaleApp.Admin.UI.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class Currency : IEntityBase
    {
        public int Id { get; set; }
        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }

        public bool Status { get; set; } = true;

        public bool Published { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsDefault { get; set; }

        public int DisplayOrder { get; set; }
    }
}