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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Presentation;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class SystemUserController : Controller
    {
        private readonly MySaleAppContext _context;

        public SystemUserController(MySaleAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg = 1, int pageSize = 9, string searchText = "")
        {
            if (pg < 1) { pg = 1; }

            var pageSizes = new[] { 5, 10, 25, 50, 100 };

            ViewData["PageSizes"] = new SelectList(pageSizes, pageSize); // second arg selects the current value
            ViewData["PageSize"] = pageSize;

            //var data = _context.SystemUsers.AsQueryable().ToList();

            var systemUserData = (from user in _context.SystemUsers
                        join userRole in _context.SystemUserRole on user.UserRoleGuid equals userRole.UserRoleGuid into userRolesGroup
                        from userRole in userRolesGroup.DefaultIfEmpty()
                            //where user.UserRoleGuid != null

                        select new SystemUserRoleViewModel
                        {
                            UserId = user.Id,
                            UserGuid = user.UserGuid,
                            UserName = user.UserName,
                            Email = user.Email,
                            Password = user.Password,
                            PasswordSalt = user.PasswordSalt,
                            IsActive = user.IsActive,
                            IsBlocked = user.IsBlocked,
                            IsDeleted = user.IsDeleted,
                            IsSystemAccount = user.IsSystemAccount,
                            LastActivityDateUtc = (DateTime)user.LastActivityDateUtc,
                            LastIpAddress = user.LastIpAddress,
                            LastLoginDateUtc = user.LastLoginDateUtc,
                            CreatedDate = user.CreatedDate,
                            CreatedBy = user.CreatedBy,
                            ModifiedDate = user.ModifiedDate,
                            ModifiedBy = user.ModifiedBy,
                            UserRoleGuid = user.UserRoleGuid,
                            UserRoleName = (userRole != null) ? userRole.UserRoleName : "Null",

                        }).ToList();

            if (systemUserData != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    systemUserData = systemUserData.Where(x => x.UserName != null && x.UserName.ToUpper().Contains(searchText.ToUpper())).ToList();

                }
            }

            // Paging logic
            int totalRecords = systemUserData.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = systemUserData.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResultViewModel
            {
                SystemUsers = data,
                PageIndex = pg,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            ViewData["PageSize"] = pageSize;

            return View(viewModel);
        }


        // GET: SystemUser/Create
        public IActionResult Create()
        {
            var newsystemUser = new SystemUser()
            {
                LastActivityDateUtc = DateTime.UtcNow,
                LastLoginDateUtc = DateTime.UtcNow
            };

            ViewBag.UserRoles = new SelectList(_context.SystemUserRole, "UserRoleGuid", "UserRoleName");

            return View(newsystemUser);
        }


        // POST: SystemUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                systemUser.UserGuid = Guid.NewGuid();
                systemUser.CreatedBy = "300196";
                systemUser.ModifiedBy = "300196";
                systemUser.CreatedDate = DateTime.Now;
                if (string.IsNullOrEmpty(systemUser.UserRoleGuid))
                {
                    systemUser.UserRoleGuid = "1"; // Default User Role Guid
                }

                ViewBag.UserRoles = new SelectList(_context.SystemUserRole, "UserRoleGuid", "UserRoleName");


                var passwordHasher = new PasswordHasher<SystemUser>();
                systemUser.Password = passwordHasher.HashPassword(systemUser, systemUser.Password);

                try
                {
                    _context.Add(systemUser);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{systemUser.UserName} registered successfully.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password.");
                    return View(systemUser);
                    
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserRoles = new SelectList(_context.SystemUserRole, "UserRoleGuid", "UserRoleName");
            return View(systemUser);
        }

        // GET: SystemUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SystemUsers == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUsers.FindAsync(id);

            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserGuid,UserName,Email,Password,PasswordSalt,isActive,isBlocked,isDeleted,IsSystemAccount,LastActivityDateUtc,LastIpAddress,LastLoginDateUtc,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,UserRoleGuid")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSystemUsers = await _context.SystemUsers.FindAsync(id);
                    if (existingSystemUsers == null)
                    {
                        return NotFound();
                    }

                    systemUser.UserGuid = existingSystemUsers.UserGuid;
                    systemUser.Password = existingSystemUsers.Password;
                    systemUser.PasswordSalt = existingSystemUsers.PasswordSalt;
                    systemUser.CreatedBy = existingSystemUsers.CreatedBy;
                    systemUser.CreatedDate = existingSystemUsers.CreatedDate;

                    systemUser.ModifiedBy = "100713";
                    systemUser.ModifiedDate = DateTime.Now;

                    _context.Entry(existingSystemUsers).CurrentValues.SetValues(systemUser);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
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
            return View(systemUser);
        }

        // GET: SystemUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SystemUsers == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (systemUser != null)
            {
                _context.SystemUsers.Remove(systemUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: SystemUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SystemUsers == null)
            {
                return Problem("Entity set 'MySaleAppContext.SystemUsers'  is null.");
            }
            var systemUser = await _context.SystemUsers.FindAsync(id);
            if (systemUser != null)
            {
                _context.SystemUsers.Remove(systemUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemUserExists(int id)
        {
            return _context.SystemUsers.Any(e => e.Id == id);
        }
    }
}
