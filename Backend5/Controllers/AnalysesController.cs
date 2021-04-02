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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext context;

        public AnalysesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Analyses
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;

            var analysis = await this.context.Analyses
                .Include(w => w.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(analysis);
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var analysis = await context.Analyses
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .SingleOrDefaultAsync(m => m.AnalysisId == analysisId && m.PatientId == patientId);

            if (analysis == null)
            {
                return NotFound();
            }

            this.ViewBag.Analysis = analysis;

            return View(analysis);
        }

        // GET: Analyses/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            return this.View(new AnalysisCreateAndEditModel());
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysisCreateAndEditModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .SingleOrDefaultAsync(x => x.Id == model.LabId);

            var analyses = await context.Analyses.ToListAsync();
            var count = analyses.Count();

            if (this.ModelState.IsValid)
            {
                var newLab = new Lab();
                newLab = lab;

                var analysis = new Analysis
                {
                    AnalysisId = ++count,
                    PatientId = patient.Id,
                    Patient = patient,
                    LabId = model.LabId,
                    Lab = newLab,
                    Type = model.Type,
                    Date = model.Date,
                    Status = model.Status
                };

                this.context.Add(analysis);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            //TODO: fix
            return this.View(model);
        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return NotFound();
            }

            var analysis = await context.Analyses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.AnalysisId == analysisId && m.PatientId == patientId);

            if (analysis == null)
            {
                return this.NotFound();
            }

            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            this.ViewBag.Analysis = analysis;

            var model = new AnalysisCreateAndEditModel
            {
                Type = analysis.Type,
                Date = analysis.Date,
                Status = analysis.Status,
                LabId = analysis.LabId == null ? -1 : (int)analysis.LabId
            };

            return this.View(model);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? analysisId, Int32? patientId, AnalysisCreateAndEditModel model)
        {
            if (analysisId == null || patientId == null)
            {
                return NotFound();
            }

            var analysis = await context.Analyses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.AnalysisId == analysisId && m.PatientId == patientId);

            if (analysis == null)
            {
                return this.NotFound();
            }

            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            this.ViewBag.Analysis = analysis;

            if (this.ModelState.IsValid)
            {
                analysis.Type = model.Type;
                analysis.Date = model.Date;
                analysis.Status = model.Status;
                analysis.LabId = model.LabId;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = analysis.PatientId });
            }

            return this.View(model);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return NotFound();
            }

            var analysis = await context.Analyses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.AnalysisId == analysisId && m.PatientId == patientId);

            if (analysis == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Analysis = analysis;

            return View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? analysisId, Int32? patientId)
        {
            var analysis = await context.Analyses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.AnalysisId == analysisId && m.PatientId == patientId);

            context.Analyses.Remove(analysis);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", new { patientId = analysis.PatientId });
        }

        private bool AnalysisExists(int id)
        {
            return context.Analyses.Any(e => e.AnalysisId == id);
        }
    }
}
