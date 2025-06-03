using Microsoft.AspNetCore.Mvc.RazorPages;
using MySaleApp.Admin.UI.Interface;
   using System;
   using System.Collections.Generic;


namespace MySaleApp.Admin.UI.Models
{
    public class PermissionRecord : IEntityBase
    {
        public int Id { get; set; }

        public string PermissionGuid { get; set; }
        public string PermissionName { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
       
    }
}