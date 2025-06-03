 ﻿using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class SystemUserRole : IEntityBase
    {
        public int Id { get; set; }

        public string? UserRoleGuid { get; set; }
        public string UserRoleName { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemRole { get; set; }

        public string? CustomerGuid { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}