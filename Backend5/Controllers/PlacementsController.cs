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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PlacementsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Placements
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

            var items = await this.context.Placements
                .Include(h => h.Ward)
                .Include(h => h.Patient)
                .Where(x => x.WardId == ward.Id)
                .ToListAsync();

            this.ViewBag.Ward = ward;
            return this.View(items);
        }

        // GET: Placements/Create
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
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name");
            return this.View(new PlacementCreateModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, PlacementCreateModel model)
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

            if (PlacementExists(ward.Id, model.PatientId))
            {
                this.ModelState.AddModelError("PatientId", "This patient is already attached to this ward");
                this.ViewBag.Ward = ward;
                this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name", model.PatientId);
                return this.View(model);
            }

            if (BedIsUsed(model.Bed, ward.Id))
            {
                this.ModelState.AddModelError("Bed", "This bed is already used another person");
                this.ViewBag.Ward = ward;
                this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name", model.PatientId);
                return this.View(model);
            }

            if (this.ModelState.IsValid)
            {
                var placement = new Placement
                {
                    WardId = ward.Id,
                    PatientId = model.PatientId,
                    Bed = model.Bed
                };

                this.context.Add(placement);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name", model.PatientId);
            return this.View(model);
        }

        
        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? wardId, Int32? patientId)
        {
            if (wardId == null || patientId == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .Include(h => h.Ward)
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.WardId == wardId && m.PatientId == patientId);

            if (placement == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Placement = placement;

            return this.View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? wardId, Int32? patientId)
        {
            var placement = await this.context.Placements
                .Include(h => h.Ward)
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.WardId == wardId && m.PatientId == patientId);

            context.Placements.Remove(placement);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = wardId });
        }

        private bool PlacementExists(Int32 wardId, Int32 patientId)
        {
            return context.Placements.Any(e => e.PatientId == patientId && e.WardId == wardId);
        }

        private bool BedIsUsed(Int32 bed, Int32 wardId)
        {
            return context.Placements.Any(e => e.Bed == bed && e.WardId == wardId);
        }
    }
}
