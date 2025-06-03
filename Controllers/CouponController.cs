using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using MySaleApp.Admin.UI.Models;
using MySaleApp.Admin.UI.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class CouponController : Controller
    {
        private readonly MySaleAppContext _context;

        public CouponController(MySaleAppContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg = 1, string searchText = "")
        {
            const int pageSize = 8;

            if (pg < 1)
                pg = 1;

            var query = _context.Coupon.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.CouponName != null && x.CouponName.ToUpper().Contains(searchText.ToUpper()));
            }

            int recsCount = query.Count();
            int recSkip = (pg - 1) * pageSize;
            var pager = new Pager(recsCount, pg, pageSize);

            var coupons = query.OrderBy(x => x.Id).Skip(recSkip).Take(pageSize).ToList();
            ViewBag.Pager = pager;

            return View(coupons);
        }

        //[HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Coupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponId,CouponName,CouponCode,DiscountAmount,DiscountPercentage,IsDiscountPercentage,StartDate,ExpirationDate,IsNotExpire,MinimumAmount,MaximumDiscountAmount,IsMultipleRedemptionAllowed,IsPlanOnlyCoupon,PlanId,IsAdditionalLicenseOnlyCoupon,IsRenewalPlanOnly,IsActive,Status,,CreatedDate,CreatedBy")] CouponViewModel couponViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var coupon = new Coupon
                    {
                        CouponId = Guid.NewGuid(),
                        CouponName = couponViewModel.CouponName,
                        CouponCode = couponViewModel.CouponCode,
                        DiscountAmount = couponViewModel.DiscountAmount,
                        DiscountPercentage = couponViewModel.DiscountPercentage,
                        IsDiscountPercentage = couponViewModel.IsDiscountPercentage,
                        StartDate = couponViewModel.StartDate,
                        ExpirationDate = couponViewModel.ExpirationDate,
                        IsNotExpire = couponViewModel.IsNotExpire,
                        MinimumAmount = couponViewModel.MinimumAmount,
                        MaximumDiscountAmount = couponViewModel.MaximumDiscountAmount,
                        IsMultipleRedemptionAllowed = couponViewModel.IsMultipleRedemptionAllowed,
                        IsPlanOnlyCoupon = couponViewModel.IsPlanOnlyCoupon,
                        PlanId = couponViewModel.PlanId,
                        IsAdditionalLicenseOnlyCoupon = couponViewModel.IsAdditionalLicenseOnlyCoupon,
                        IsRenewalPlanOnly = couponViewModel.IsRenewalPlanOnly,
                        IsActive = couponViewModel.IsActive,
                        Status = couponViewModel.Status,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "300196",
                        //ModifiedDate = DateTime.Now,
                        //ModifiedBy = User.Identity.Name
                    };

                    _context.Add(coupon);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Successfully Saved";
                    ModelState.Clear();
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the plan. Details: " + ex.Message);
                }

            }
            return View(couponViewModel);
        }

    }
}
