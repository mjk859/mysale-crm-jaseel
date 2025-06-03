using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using MySaleApp.Admin.UI.Models;
using MySaleApp.Admin.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly MySaleAppContext _context;

        public UserController(MySaleAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg = 1, int pageSize = 10, string searchType = "", string searchQuery = "", DateTime? fromDate = null, DateTime? toDate = null, int? pageNumber = null)
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };

            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;
            var Customers = await _context.Customers.ToListAsync();
            var Users = await _context.Users.ToListAsync();

            var userData = (from user in Users
                            join customer in Customers on user.ParentCustomerId equals customer.CustomerGuid.ToString()

                            select new CustomerUserViewModel
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                UserName = user.UserName ?? string.Empty,
                                CountryDialCode = user.CountryDialCode ?? string.Empty,
                                MobileNo = user.MobileNo ?? string.Empty,
                                MobileVerificationOtp = user.MobileVerificationOtp ?? string.Empty,
                                IsMobileVerified = user.IsMobileVerified,
                                Email = user.Email ?? string.Empty,
                                EmailVerificationOtp = user.EmailVerificationOtp ?? string.Empty,
                                IsEmailVerified = user.IsEmailVerified,
                                IsActive = user.IsActive,
                                IsBlocked = user.IsBlocked,
                                IsDeleted = user.IsDeleted,
                                IsSystemAccount = user.IsSystemAccount,
                                LastActivityDateUtc = user.LastActivityDateUtc ?? DateTime.MinValue,
                                LastLoginDateUtc = user.LastLoginDateUtc,
                                CreatedDate = user.CreatedDate,
                                CreatedBy = user.CreatedBy ?? string.Empty,
                                ModifiedDate = user.ModifiedDate,
                                ModifiedBy = user.ModifiedBy ?? string.Empty,
                                ParentCustomerId = user.ParentCustomerId ?? string.Empty,
                                CustomerName = customer.CustomerName ?? string.Empty
                            }).ToList();

            if (userData != null)
            {
                switch (searchType)
                {
                    case "UserGuid":
                        userData = userData.Where(x => x.UserGuid.ToString() == searchQuery).ToList();
                        break;

                    case "ParentCustomerId":
                        userData = userData.Where(x => x.ParentCustomerId.ToString() == searchQuery).ToList();
                        break;

                    case "UserName":
                        userData = userData.Where(x => x.UserName != null && x.UserName.ToUpper().Contains(searchQuery.ToUpper())).ToList();
                        break;

                    case "MobileNo":
                        userData = userData.Where(x => x.MobileNo.Contains(searchQuery)).ToList();
                        break;

                    case "MobileVerificationOtp":
                        userData = userData.Where(x => x.MobileVerificationOtp.Contains(searchQuery)).ToList();
                        break;

                    case "isMobileVerified":
                        userData = userData.Where(x => x.IsMobileVerified.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "Email":
                        userData = userData.Where(x => x.Email.Contains(searchQuery)).ToList();
                        break;

                    case "EmailVerificationOtp":
                        userData = userData.Where(x => x.EmailVerificationOtp.Contains(searchQuery)).ToList();
                        break;

                    case "isEmailVerified":
                        userData = userData.Where(x => x.IsEmailVerified.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "Password":
                        userData = userData.Where(x => x.Password.Contains(searchQuery)).ToList();
                        break;

                    case "PasswordSalt":
                        userData = userData.Where(x => x.PasswordSalt.Contains(searchQuery)).ToList();
                        break;

                    case "isPasswordCreated":
                        userData = userData.Where(x => x.IsPasswordCreated.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "isActive":
                        userData = userData.Where(x => x.IsActive.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "isBlocked":
                        userData = userData.Where(x => x.IsBlocked.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "isDeleted":
                        userData = userData.Where(x => x.IsDeleted.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "CreatedDate":
                        userData = userData.Where(x => x.CreatedDate.Date == fromDate).ToList();
                        break;

                    case "CreatedBy":
                        userData = userData.Where(x => x.CreatedBy != null && x.CreatedBy.Contains(searchQuery)).ToList();
                        break;

                    case "ModifiedDate":
                        userData = userData.Where(x => x.ModifiedDate.Date == fromDate).ToList();
                        break;

                    case "ModifiedBy":
                        userData = userData.Where(x => x.ModifiedBy != null && x.ModifiedBy.Contains(searchQuery)).ToList();
                        break;

                    case "LastActivityDateUtc":
                        userData = userData.Where(x => x.LastActivityDateUtc == fromDate).ToList();
                        break;

                    case "LastIpAddress":
                        userData = userData.Where(x => x.LastIpAddress.Contains(searchQuery)).ToList();
                        break;

                    case "LastLoginDateUtc":
                        userData = userData.Where(x => x.LastLoginDateUtc == fromDate).ToList();
                        break;

                    case "IsSystemAccount":
                        userData = userData.Where(x => x.IsSystemAccount.ToString().ToUpper() == searchQuery.ToUpper()).ToList();
                        break;

                    case "CountryDialCode":
                        userData = userData.Where(x => x.CountryDialCode != null && x.CountryDialCode.Contains(searchQuery)).ToList();
                        break;

                    default:
                        userData = userData.ToList();
                        break;
                }
            }

            int totalRecords = userData?.Count ?? 0;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
             var data = (userData ?? new List<CustomerUserViewModel>())
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PagedResultViewModel
            {
                Users = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }
    }
}