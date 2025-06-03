using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using MySaleApp.Admin.UI.ViewModel;
using System.Globalization;
using System.Text.Json;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly MySaleAppContext _context;
        private readonly ILogger<DashboardController> _logger;
        private readonly IDistributedCache _cache;

        public DashboardController(MySaleAppContext context, ILogger<DashboardController> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"Dashboard accessed. Auth status: {User?.Identity?.IsAuthenticated ?? false}");
            Console.WriteLine($"Request URL: {Request?.Path ?? "Unknown"}");
            Console.WriteLine($"Full URL: {Request?.Scheme}://{Request?.Host}{Request?.Path}");

            try
            {
                
                var today = DateTime.Today;
                var last7Days = Enumerable.Range(0, 7)
                    .Select(i => today.AddDays(-i))
                    .ToList();

                var startDate = last7Days.Min();
                var endDate = last7Days.Max().AddDays(1);

                var groupedSignups = await _context.Customers
                    .Where(c => c.CreatedDate >= startDate && c.CreatedDate < endDate)
                    .GroupBy(c => c.CreatedDate.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .ToListAsync();

                var signupCountsDict = groupedSignups.ToDictionary(g => g.Date, g => g.Count);

                var signupCounts = last7Days
                    .Select(date => signupCountsDict.ContainsKey(date) ? signupCountsDict[date] : 0)
                    .ToList();

                var recentCustomers = await _context.Customers
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(5)
                    .Select(c => new CustomerInfo
                    {
                        Name = c.CustomerName,
                        Email = c.Email,
                        DateCreated = c.CreatedDate,
                    })
                    .ToListAsync();

                var dashboardViewModel = new DashboardViewModel
                {
                    TotalCustomers = await _context.Customers.CountAsync(),
                    TotalSystemUsers = await _context.SystemUsers.CountAsync(),
                    ActivePlans = await _context.Customers.CountAsync(c => c.IsActive),
                    ActiveCoupons = await _context.Coupon.CountAsync(c => c.IsActive),
                    CustomerSignupDates = last7Days
                        .OrderBy(d => d)
                        .Select(d => d.ToString("MMM dd", CultureInfo.InvariantCulture))
                        .ToList(),
                    CustomerSignupCounts = signupCounts,
                    RecentCustomers = recentCustomers
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard");
                return View("Error");
            }
        }
    }
}
