using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext context;

        public WardStaffsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;

            var wardStaff = await this.context.WardStafs
                .Include(w => w.Ward)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(wardStaff);
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(Int32? wardStaffId, Int32? wardId)
        {
            if (wardStaffId == null || wardId ==  null)
            {
                return NotFound();
            }

            var wardStaff = await context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.WardStaffId == wardStaffId && m.WardId == wardId);

            if (wardStaff == null)
            {
                return NotFound();
            }

            this.ViewBag.WardStaff = wardStaff;

            return View(wardStaff);
        }

        // GET: WardStaffs/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {

            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            return this.View(new WardStaffCreateAndEditModel());

        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStaffCreateAndEditModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            var wardStafs = await context.WardStafs.ToListAsync();
            var count = wardStafs.Count();

            if (this.ModelState.IsValid)
            {
                var wardStaff = new WardStaff
                {
                    WardStaffId = ++count,
                    WardId = ward.Id,
                    Ward = ward,
                    Name = model.Name,
                    Position = model.Position
                };

                this.context.Add(wardStaff);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            return this.View(model);
        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(Int32? wardStaffId, Int32? wardId)
        {

            if (wardStaffId == null || wardId == null)
            {
                return NotFound();
            }

            var wardStaff = await context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.WardStaffId == wardStaffId && m.WardId == wardId);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            this.ViewBag.WardStaff = wardStaff;

            var model = new WardStaffCreateAndEditModel
            {
                Name = wardStaff.Name,
                Position = wardStaff.Position
            };

            return this.View(model);

        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? wardStaffId, Int32? wardId, WardStaffCreateAndEditModel model)
        {

            if (wardStaffId == null || wardId == null)
            {
                return NotFound();
            }

            var wardStaff = await context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.WardStaffId == wardStaffId && m.WardId == wardId);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            this.ViewBag.WardStaff = wardStaff;

            if (this.ModelState.IsValid)
            {
                wardStaff.Name = model.Name;
                wardStaff.Position = model.Position;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
            }

            return this.View(model);
        }

        // GET: WardStaffs/Delete/5
        public async Task<IActionResult> Delete(Int32? wardStaffId, Int32? wardId)
        {

            if (wardStaffId == null || wardId == null)
            {
                return NotFound();
            }

            var wardStaff = await context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.WardStaffId == wardStaffId && m.WardId == wardId);

            if (wardStaff == null)
            {
                return NotFound();
            }

            this.ViewBag.WardStaff = wardStaff;

            return View(wardStaff);
        }

        // POST: WardStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? wardStaffId, Int32? wardId)
        {
            var wardStaff = await context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.WardStaffId == wardStaffId && m.WardId == wardId);

            this.ViewBag.WardStaff = wardStaff;

            context.WardStafs.Remove(wardStaff);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = wardStaff.WardId });
        }

        //private bool WardStaffExists(int id)
        //{
        //    return context.WardStafs.Any(e => e.Id == id);
        //} TODO: мб пригодится
        
        
    }
}
