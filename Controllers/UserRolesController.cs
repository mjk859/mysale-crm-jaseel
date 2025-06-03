using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MySaleApp.Admin.UI.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Presentation;
using MySaleApp.Admin.UI.ViewModel;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly MySaleAppContext _context;

        public UserRolesController(MySaleAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg = 1, int pageSize = 9, string searchText = "")
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };

            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            var userRoleData = _context.SystemUserRole.AsQueryable().ToList();

            if (userRoleData != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    userRoleData = userRoleData.Where(x => x.UserRoleName != null && x.UserRoleName.ToUpper().Contains(searchText.ToUpper())).ToList();

                }
            }

            // Paging logic
            int totalRecords = userRoleData.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = userRoleData.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResultViewModel
            {
                UserRoles = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerGuid", "CustomerName");
            return View();
        }


        // POST: UserRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUserRole UserRoles)
        {
            UserRoles.UserRoleGuid = Guid.NewGuid().ToString();
            UserRoles.CreatedBy = "300196";
            UserRoles.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(UserRoles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerGuid", "CustomerName");


            Console.WriteLine($"CustomerGuid: {UserRoles.CustomerGuid}");
            Console.WriteLine($"UserRoleName: {UserRoles.UserRoleName}");


            return View(UserRoles);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SystemUserRole == null)
            {
                return NotFound();
            }

            var UserRoles = await _context.SystemUserRole.FindAsync(id);

            if (UserRoles == null)
            {
                return NotFound();
            }
            return View(UserRoles);
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserRoleGuid,UserRoleName,SystemName,IsSystemRole,CreatedBy,CreatedDate")] SystemUserRole UserRoles)
        {
            if (id != UserRoles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUserRoles = await _context.SystemUserRole.FindAsync(id);
                    if (existingUserRoles == null)
                    {
                        return NotFound();
                    }

                    UserRoles.UserRoleGuid = existingUserRoles.UserRoleGuid;
                    UserRoles.CreatedBy = existingUserRoles.CreatedBy;
                    UserRoles.CreatedDate = existingUserRoles.CreatedDate;

                    _context.Entry(existingUserRoles).CurrentValues.SetValues(UserRoles);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRolesExists(UserRoles.Id))
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
            return View(UserRoles);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var UserRoles = await _context.SystemUserRole.FirstOrDefaultAsync(m => m.Id == id);

            if (UserRoles != null)
            {
                _context.SystemUserRole.Remove(UserRoles);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SystemUserRole == null)
            {
                return Problem("Entity set 'MySaleAppContext.UserRoles' is null");
            }
            var UserRoles = await _context.SystemUserRole.FindAsync(id);

            if (UserRoles != null)
            {
                _context.SystemUserRole.Remove(UserRoles);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRolesExists(int id)
        {
            return _context.SystemUserRole.Any(e => e.Id == id);
        }

        //Get
        public IActionResult GetUserRoles()
        {
            List<SystemUserRole> userRoles = _context.SystemUserRole.ToList();

            //List<UserRoleViewModel> userRoles = _context.UserRoles
            //    .Select(userRole => new UserRoleViewModel
            //    {
            //        UserRoleId = userRole.Id,
            //        UserRoleGuid = userRole.UserRoleGuid,
            //        UserRoleName = userRole.UserRoleName
            //    }).ToList();

            return Json(userRoles);
        }
    }
}
