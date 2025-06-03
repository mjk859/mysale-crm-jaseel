using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Model;
using MySaleApp.Admin.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaleApp.Admin.UI.Controllers
{
    [Authorize]
    public class PermissionRecordController : Controller
    {
        private readonly MySaleAppContext _context;

        public PermissionRecordController(MySaleAppContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg = 1, string searchText = "")
        {
            const int pageSize = 9;

            if (pg < 1)
                pg = 1;

            var data = _context.PermissionRecords.AsQueryable().ToList();

            if (data != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    data = data.Where(x => x.PermissionName != null && x.PermissionName.ToUpper().Contains(searchText.ToUpper())).ToList();

                }
            }

            int recsCount = data.Count();
            int recSkip = (pg - 1) * pageSize;
            var pager = new Pager(recsCount, pg, pageSize);

            List<PermissionRecord> permissionRecords = data.OrderBy(x => x.Id).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(permissionRecords);
        }


        // GET: PermissionRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PermissionRecords/Creat    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PermissionGuid,PermissionName,SystemName,Category,Status,CreatedBy,CreatedDate")] PermissionRecord permissionRecord)
        {
            if (ModelState.IsValid)
            {
                permissionRecord.PermissionGuid = Guid.NewGuid().ToString();
                permissionRecord.CreatedBy = "300196";
                permissionRecord.CreatedDate = DateTime.Now;

                _context.Add(permissionRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permissionRecord);
        }

        // GET: PermissionRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PermissionRecords == null)
            {
                return NotFound();
            }

            var permissionRecord = await _context.PermissionRecords.FindAsync(id);

            if (permissionRecord == null)
            {
                return NotFound();
            }
            return View(permissionRecord);
        }

        // POST: PermissionRecords/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermissionGuid,PermissionName,SystemName,Category,Status,CreatedBy,CreatedDate")] PermissionRecord permissionRecord)
        {
            if (id != permissionRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingpermissionRecord = await _context.PermissionRecords.FindAsync(id);
                    if (existingpermissionRecord == null)
                    {
                        return NotFound();
                    }

                    permissionRecord.PermissionGuid = existingpermissionRecord.PermissionGuid;
                    permissionRecord.CreatedBy = existingpermissionRecord.CreatedBy;
                    permissionRecord.CreatedDate = existingpermissionRecord.CreatedDate;

                    _context.Entry(existingpermissionRecord).CurrentValues.SetValues(permissionRecord);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionRecordExists(permissionRecord.Id))
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
            return View(permissionRecord);
        }

        // GET: PermissionRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PermissionRecords == null)
            {
                return NotFound();
            }

            var permissionRecord = await _context.PermissionRecords.FirstOrDefaultAsync(m => m.Id == id);

            if (permissionRecord != null)
            {
                _context.PermissionRecords.Remove(permissionRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // POST: PermissionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PermissionRecords == null)
            {
                return Problem("Entity set 'MySaleAppContext.PermissionRecords'  is null.");
            }
            var permissionRecord = await _context.PermissionRecords.FindAsync(id);
            if (permissionRecord != null)
            {
                _context.PermissionRecords.Remove(permissionRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionRecordExists(int id)
        {
            return _context.PermissionRecords.Any(e => e.Id == id);
        }
    }
}
