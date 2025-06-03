using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.ViewModel;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly MySaleAppContext _context;

        public CustomerController(MySaleAppContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index(int pg = 1, int pageSize = 10, string searchType = "", string searchQuery = "", DateTime? fromDate = null, DateTime? toDate = null, int? searchDays = null, int? pageNumber = null)
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };
            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            var customerQuery = (from c in _context.Customers
                                 join p in _context.Plans on c.PlanId equals p.PlanGuid.ToString() into cu_pl
                                 from p in cu_pl.DefaultIfEmpty()
                                 select new CustomerPlanViewModel
                                 {
                                     Id = c.Id,
                                     CustomerGuid = c.CustomerGuid,
                                     CustomerName = c.CustomerName ?? string.Empty,
                                     // DbName = c.DbName,
                                     MobileNo = c.MobileNo ?? string.Empty,
                                     IsMobileVerified = c.IsMobileVerified,
                                     Email = c.Email ?? string.Empty,
                                     EmailVerificationOtp = c.EmailVerificationOtp ?? string.Empty,
                                     IsEmailVerified = c.IsEmailVerified,
                                     CountryCode = c.CountryCode ?? string.Empty,
                                     IsActive = c.IsActive,
                                     Licenses = c.Licenses,
                                     AdditionalLicense = c.AdditionalLicense,
                                     IsBlocked = c.IsBlocked,
                                     IsDeleted = c.IsDeleted,
                                     IsDbCreated = c.IsDbCreated,
                                     IsDbProcessCompleted = c.IsDbProcessCompleted,
                                     ReferralCode = c.ReferralCode ?? string.Empty,
                                     ReferredBy = c.ReferredBy ?? string.Empty,
                                     CreatedBy = c.CreatedBy ?? string.Empty,
                                     CreatedDate = c.CreatedDate,
                                     ModifiedBy = c.ModifiedBy ?? string.Empty,
                                     ModifiedDate = c.ModifiedDate,
                                     PlanExpiryDate = c.PlanExpiryDate,
                                     Platform = c.Platform ?? string.Empty,
                                     PlanName = p.Name ?? string.Empty
                                 });

            if (!string.IsNullOrEmpty(searchQuery))
            {
                switch (searchType)
                {
                    case "CustomerName":
                        customerQuery = customerQuery.Where(x => x.CustomerName != null && x.CustomerName.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "CustomerGuid":
                        customerQuery = customerQuery.Where(x => x.CustomerGuid.ToString().ToUpper() == searchQuery);
                        break;

                    case "DbName":
                        customerQuery = customerQuery.Where(x => x.DbName != null && x.DbName.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "MobileNo":
                        customerQuery = customerQuery.Where(x => x.MobileNo.Contains(searchQuery));
                        break;

                    case "MobileVerificationOtp":
                        customerQuery = customerQuery.Where(x => x.MobileVerificationOtp != null && x.MobileVerificationOtp.Contains(searchQuery));
                        break;

                    case "isMobileVerified":
                        customerQuery = customerQuery.Where(x => x.IsMobileVerified.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "Email":
                        customerQuery = customerQuery.Where(x => x.Email != null && x.Email.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "EmailVerificationOtp":
                        customerQuery = customerQuery.Where(x => x.EmailVerificationOtp != null && x.EmailVerificationOtp.Contains(searchQuery));
                        break;

                    case "isEmailVerified":
                        customerQuery = customerQuery.Where(x => x.IsEmailVerified.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "CountryCode":
                        customerQuery = customerQuery.Where(x => x.CountryCode.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "CountryDialCode":
                        customerQuery = customerQuery.Where(x => x.CountryDialCode.Contains(searchQuery));
                        break;

                    case "isActive":
                        customerQuery = customerQuery.Where(x => x.IsActive.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "isBlocked":
                        customerQuery = customerQuery.Where(x => x.IsBlocked.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "isDeleted":
                        customerQuery = customerQuery.Where(x => x.IsDeleted.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "referralCode":
                        customerQuery = customerQuery.Where(x => x.ReferralCode.Contains(searchQuery));
                        break;

                    case "referredBy":
                        customerQuery = customerQuery.Where(x => x.ReferredBy.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "CreatedDate":
                        customerQuery = customerQuery.Where(x => x.CreatedDate.Date == fromDate);
                        break;

                    case "CreatedBy":
                        customerQuery = customerQuery.Where(x => x.CreatedBy != null && x.CreatedBy.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "ModifiedDate":
                        customerQuery = customerQuery.Where(x => x.ModifiedDate.Date == fromDate);
                        break;

                    case "ModifiedBy":
                        customerQuery = customerQuery.Where(x => x.ModifiedBy != null && x.ModifiedBy.ToUpper().Contains(searchQuery.ToUpper()));
                        break;

                    case "isDbCreated":
                        customerQuery = customerQuery.Where(x => x.IsDbCreated.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "isDbProcessCompleted":
                        customerQuery = customerQuery.Where(x => x.IsDbProcessCompleted.ToString().ToUpper() == searchQuery.ToUpper());
                        break;

                    case "PlanId":
                        customerQuery = customerQuery.Where(x => x.PlanId == searchQuery);
                        break;

                    case "ExpiryDate":

                        if (fromDate.HasValue && toDate.HasValue && searchDays.HasValue)
                        {
                            var fromDateRange = fromDate.Value.Date;
                            var toDateRange = toDate.Value.Date;
                            var now = DateTime.Now.Date;
                            customerQuery = customerQuery.Where(x => x.PlanExpiryDate.HasValue &&
                                                                    x.PlanExpiryDate.Value.Date >= fromDateRange &&
                                                                    x.PlanExpiryDate.Value.Date <= toDateRange &&
                                                                    ((x.PlanExpiryDate.Value.Date - now).TotalDays <= searchDays));
                        }
                        else if (searchDays.HasValue)
                        {
                            var now = DateTime.Now.Date;
                            customerQuery = customerQuery.Where(x => x.PlanExpiryDate.HasValue &&
                                                                   (x.PlanExpiryDate.Value.Date - now).TotalDays <= searchDays);
                        }

                        break;

                    default:
                        break;
                }
            }

            // Paging logic
            int totalRecords = await customerQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = await customerQuery
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new PagedResultViewModel
            {
                Customers = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("CustomerName"),
                                        new DataColumn("CountryCode"),
                                        new DataColumn("MobileNo"),
                                        new DataColumn("Plan Name"),
                                        new DataColumn("PlanExpiryDate"),
                                        new DataColumn("License"),
                                        new DataColumn("Additional License"),
                                        new DataColumn("CreatedDate")
            });
            //from c in categories
            //join pt in products on c.Category equals pt.Category into ps_jointable
            //from p in ps_jointable.DefaultIfEmpty()
            //select new { Category = c, ProductName = p == null ? "(No products)" : p.ProductName };

            var customers = from cust in this._context.Customers
                            join plan in this._context.Plans on cust.PlanId equals plan.PlanGuid.ToString() into cu_pl
                            from pc in cu_pl.DefaultIfEmpty()
                            select new
                            {
                                CustomerName = cust.CustomerName,
                                CountryCode = cust.CountryCode,
                                MobileNo = cust.MobileNo,
                                PlanName = pc.Name,
                                PlanExpiryDate = cust.PlanExpiryDate,
                                License = cust.Licenses,
                                AdditionalLicense = cust.AdditionalLicense,
                                CreatedDate = cust.CreatedDate
                            };

            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.CustomerName, customer.CountryCode, customer.MobileNo, customer.PlanName, customer.PlanExpiryDate, customer.License, customer.AdditionalLicense, customer.CreatedDate);
            }
            var TotalPlans = customers.GroupBy(n => n.PlanName)
                    .Select(c => new { plan = c.Key, count = c.Count() });
            dt.Rows.Add("", "", "", "", "", "", "", "");
            dt.Rows.Add("", "", "", "", "", "", "", "");

            foreach (var tp in TotalPlans)
            {
                dt.Rows.Add("", tp.plan, "", "", tp.count, "", "", "");
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"mysale-{DateTime.Now.ToShortDateString()}.xlsx");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Users(Guid customerGuid)
        {
            var users = await _context.Users
                .Where(u => u.ParentCustomerId == customerGuid.ToString())
                .ToListAsync();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerGuid == customerGuid);

            ViewBag.CustomerName = customer?.CustomerName ?? "Unknown Customer";

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExtendExpiry(Guid customerGuid)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerGuid == customerGuid);
            if (customer == null)
                return NotFound();

            customer.PlanExpiryDate = customer.PlanExpiryDate.AddDays(7);
            _context.SaveChanges();

            return Ok();
        }
    }
}