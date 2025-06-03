 ﻿using MySaleApp.Admin.UI.Interface;
using System;

namespace MySaleApp.Admin.UI.Models
{
    public class OtpLogs : IEntityBase
    {
        public int Id { get; set; }
        public string Orgin { get; set; }
        public string MobileNo { get; set; }
        public string SMSOtp { get; set; }
        public string SMSMessage { get; set; }
        public string SMSResponse { get; set; }
        public string Email { get; set; }
        public string EmailOtp { get; set; }
        public string EmailMessage { get; set; }
        public string EmailResponse { get; set; }
        public string Platform { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}