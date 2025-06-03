using MySaleApp.Admin.UI.Models;

namespace MySaleApp.Admin.UI.ViewModel
{
    //public class DashboardViewModel
    //{
    //    public int TotalCustomers { get; set; }
    //    public int NewCustomers { get; set; }
    //    public int NewRegisters { get; set; }
    //}

    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalSystemUsers { get; set; }
        public int ActivePlans { get; set; }
        public int ActiveCoupons { get; set; }

        public List<string> CustomerSignupDates { get; set; } = new();
        public List<int> CustomerSignupCounts { get; set; } = new();

        public List<CustomerInfo>? RecentCustomers { get; set; } = new();
    }

    public class CustomerInfo
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
