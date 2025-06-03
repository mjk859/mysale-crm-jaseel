using System;

namespace MySaleApp.Admin.UI.Models
{
    public class LoggedUser
    {
        public Guid Id { get; set; }
        public String Tocken { get; set; }
        public String UserId { get; set; }
        public String dbName { get; set; }
        public DateTime LoggedDate { get; set; }
        public DateTime LoggedExpiryDate { get; set; }
        public String IpAddress { get; set; }
        public String DeviceId { get; set; }
    }
}