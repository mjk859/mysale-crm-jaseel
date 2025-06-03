 ﻿using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class PermissionSystemUserRoleMapping : IEntityBase
    {
        public int Id { get; set; }
        public string PermissionRecordId { get; set; }
        public string UserRoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}