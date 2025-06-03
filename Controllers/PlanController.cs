using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using MySaleApp.Admin.UI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Presentation;
using MySaleApp.Admin.UI.ViewModel;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]   
    public class PlanController : Controller
    {
        private readonly MySaleAppContext _context;

        public PlanController(MySaleAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg = 1, int pageSize = 10, string searchText = "")
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };

            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            var planData = await _context.Plans.AsQueryable().ToListAsync();

            if (planData != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    planData = planData.Where(x => x.Name != null && x.Name.ToUpper().Contains(searchText.ToUpper())).ToList();
                }
            }

            // Paging logic
            int totalRecords = planData?.Count ?? 0;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = planData!.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResultViewModel
            {
                Plans = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }

        //GET: Plans/Create
        public IActionResult Create()
        {
            //TempData.Clear();
            //ModelState.Clear();
            var newPlan = new Plan()
            {
                OfferStart = DateTime.Now,
                OfferEnd = DateTime.Now,

                //CreatedDate = DateTime.Now,
                //ModifiedDate = DateTime.Now,
            };
            return View(newPlan);
            //return View();
        }

        // POST: Plans/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,PlanGuid,Name,MonthlyPrice,AnnualPrice,isBilledMonthly,LifeTimePrice,isCustomPlan,isLifeTimePlan,IsRecommended,isTrial,TrialDays,OfferPercentage,OfferStart,OfferEnd,CompaniesAllowed,UsersAllowed,DisplayOrder,Active,LogoPath,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    plan.PlanGuid = Guid.NewGuid();
                    plan.CreatedBy = "300196";
                    plan.CreatedDate = DateTime.Now;

                    _context.Add(plan);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Successfully Saved";
                    ModelState.Clear();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the plan." + ex.Message);
                }
            }
            else
            {
                //// Get validation error messages and add them to TempData or ViewBag
                //var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                //TempData["ErrorMessage"] = string.Join("<br />", errorMessages);
            }

            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Plans == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanGuid,Name,MonthlyPrice,AnnualPrice,isBilledMonthly,LifeTimePrice,isCustomPlan,isLifeTimePlan,IsRecommended,isTrial,TrialDays,OfferPercentage,OfferStart,OfferEnd,CompaniesAllowed,UsersAllowed,DisplayOrder,Active,LogoPath,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPlan = await _context.Plans.FindAsync(id);
                    if (existingPlan == null)
                    {
                        return NotFound();
                    }

                    plan.PlanGuid = existingPlan.PlanGuid;
                    plan.CreatedBy = existingPlan.CreatedBy;
                    plan.CreatedDate = existingPlan.CreatedDate;

                    plan.ModifiedBy = "100713";
                    plan.ModifiedDate = DateTime.Now;

                    _context.Entry(existingPlan).CurrentValues.SetValues(plan);
                    await _context.SaveChangesAsync();

                    //TempData["SuccessMessage"] = "Successfully Updated";
                    //ModelState.Clear();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
                return NotFound();

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }

        //Get
        public IActionResult GetPlans()
        {
            List<Plan> plans = _context.Plans.ToList();
            return Json(plans);
        }
    }
}