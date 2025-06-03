using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using MySaleApp.Admin.UI.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MySaleApp.Admin.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Presentation;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class PlanDetailsController : Controller
    {
        private readonly MySaleAppContext _context;

        public PlanDetailsController(MySaleAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg = 1, int pageSize = 9, string searchText = "")
        {

            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };

            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            var planDetailsData = (from plandetails in _context.PlanDetails
                        join plan in _context.Plans on plandetails.PlanId equals plan.Id into planGroup
                        from plan in planGroup.DefaultIfEmpty()
                        select new PlanDetailsViewModel
                        {
                            Id = plandetails.Id,
                            PlanId = plandetails.Id,
                            PlanName = plan.Name,
                            Description = plandetails.Description,
                            IsBold = plandetails.isBold,
                            IsUnderLine = plandetails.isUnderLine,
                            IsItalic = plandetails.isItalic,
                            Color = plandetails.Color,
                            //Price = plandetails.Price,
                            Active = plandetails.Active,
                            CreatedBy = plandetails.CreatedBy,
                            CreatedDate = plandetails.CreatedDate,
                            ModifiedBy = plandetails.ModifiedBy,
                            ModifiedDate = plandetails.ModifiedDate,

                        }).ToList();





            if (planDetailsData != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    planDetailsData = planDetailsData.Where(x => x.PlanName != null && x.PlanName.ToUpper().Contains(searchText.ToUpper())).ToList();

                }
            }

            // Paging logic
            int totalRecords = planDetailsData.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = planDetailsData.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResultViewModel
            {
                PlanDetails = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }

        //GET: PlanDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanId,Description,isBold,isItalic,isUnderLine,Color,Price,Active,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] PlanDetails planDetails)
        {
            if (ModelState.IsValid)
            {
                planDetails.CreatedBy = "300196";
                planDetails.CreatedDate = DateTime.Now;

                _context.Add(planDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planDetails);
        }

        // GET: PlanDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlanDetails == null)
            {
                return NotFound();
            }

            var planDetails = await _context.PlanDetails.FindAsync(id);

            if (planDetails == null)
            {
                return NotFound();
            }
            return View(planDetails);
        }

        // POST: PlanDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanId,Description,isBold,isItalic,isUnderLine,Color,Price,Active,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] PlanDetails planDetails)
        {
            if (id != planDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingplanDetails = await _context.PlanDetails.FindAsync(id);
                    if (existingplanDetails == null)
                    {
                        return NotFound();
                    }

                    planDetails.CreatedBy = existingplanDetails.CreatedBy;
                    planDetails.CreatedDate = existingplanDetails.CreatedDate;

                    planDetails.ModifiedBy = "100713";
                    planDetails.ModifiedDate = DateTime.Now;

                    _context.Entry(existingplanDetails).CurrentValues.SetValues(planDetails);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanDetailsExists(planDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(planDetails);
        }

        // GET: PlanDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlanDetails == null)
            {
                return NotFound();
            }

            var planDetails = await _context.PlanDetails.FirstOrDefaultAsync(m => m.Id == id);

            if (planDetails != null)
            {
                _context.PlanDetails.Remove(planDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: PlanDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlanDetails == null)
            {
                return Problem("Entity set 'MySaleAppContext.PlanDetails'  is null.");
            }
            var planDetails = await _context.PlanDetails.FindAsync(id);

            if (planDetails != null)
            {
                _context.PlanDetails.Remove(planDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanDetailsExists(int id)
        {
            return _context.PlanDetails.Any(e => e.Id == id);
        }
    }
}
