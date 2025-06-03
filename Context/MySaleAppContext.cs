using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Models;

namespace MySaleApp.Admin.UI.Context
{
    public class MySaleAppContext : DbContext
    {
        public MySaleAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<PermissionRecord> PermissionRecords { get; set; }
        public DbSet<SystemUserRole> SystemUserRole { get; set; }
        public DbSet<PermissionSystemUserRoleMapping> PermissionSystemUserRoleMapping { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDetails> PlanDetails { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentLogs> PaymentLogs { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<OtpLogs> OtpLogs { get; set; }
        public DbSet<Dealer> Dealer { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<CustomerCouponMapping> CustomerCouponMapping { get; set; }
    }
}