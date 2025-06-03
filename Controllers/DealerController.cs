using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using MySaleApp.Admin.UI.ViewModel;
using System.Numerics;
using System.Threading.Tasks;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class DealerController : Controller
    {
        private readonly MySaleAppContext _context;

        public DealerController(MySaleAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg = 1, int pageSize = 10, string searchText = "")
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };
            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            var dealerData = await _context.Dealer.ToListAsync();

            if (dealerData != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    dealerData = dealerData.Where(x => x.Name != null && x.Name.ToUpper().Contains(searchText.ToUpper())).ToList();
                }
            }

            // Paging logic
            int totalRecords = dealerData?.Count ?? 0;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = dealerData!.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResultViewModel
            {
                Dealers = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dealer.CreatedBy = User.Identity?.Name ?? "system";
                    dealer.CreatedDate = dealer.CreatedDate == default ? DateTime.Now : dealer.CreatedDate;
                    dealer.ModifiedBy = User.Identity?.Name ?? "system";
                    dealer.ModifiedDate = DateTime.Now;

                    _context.Add(dealer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error if needed
                    ViewBag.ErrorMessage = "An unexpected error occurred while creating the dealer. Please try again later." + ex.Message;
                }
            }
            else
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Error for {modelState.Key}: {error.ErrorMessage}");
                    }
                }

                ViewBag.ErrorMessage = "Invalid input. Please correct the highlighted fields.";
            }

            return View(dealer);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dealer == null)
            {
                return NotFound();
            }

            var dealer = await _context.Dealer.FindAsync(id);

            if (dealer == null)
            {
                return NotFound();
            }

            dealer.ModifiedDate = DateTime.Now;

            return View(dealer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DealerCode,ReferralCode,IsActive")] Dealer dealer)
        {
            if (id != dealer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDealer = await _context.Dealer.FindAsync(id);
                    if (existingDealer == null)
                    {
                        return NotFound();
                    }

                    // Only update editable fields
                    existingDealer.Name = dealer.Name;
                    existingDealer.DealerCode = dealer.DealerCode;
                    existingDealer.ReferralCode = dealer.ReferralCode;
                    existingDealer.IsActive = dealer.IsActive;

                    existingDealer.ModifiedDate = DateTime.Now;
                    existingDealer.ModifiedBy = User?.Identity?.Name ?? "system";

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealerExists(dealer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(dealer);
        }
        private bool DealerExists(int id)
        {
            return _context.Dealer.Any(e => e.Id == id);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dealer = await _context.Dealer.FindAsync(id);
            if (dealer == null)
                return NotFound();

            _context.Dealer.Remove(dealer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
