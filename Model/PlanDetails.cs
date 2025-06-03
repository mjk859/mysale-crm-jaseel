 ﻿using MySaleApp.Admin.UI.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySaleApp.Admin.UI.Models
{
    public class PlanDetails : IEntityBase
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public string Description { get; set; }
        public string HtmlData { get; set; }//Dispaly Html Data
        public bool isBold { get; set; }
        public bool isItalic { get; set; }
        public bool isUnderLine { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ModifiedBy { get; set; }
    }
}