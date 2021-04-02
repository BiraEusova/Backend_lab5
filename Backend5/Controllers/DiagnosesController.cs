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
    public class DiagnosesController : Controller
    {
        private readonly ApplicationDbContext context;

        public DiagnosesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Diagnoses
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

            var diagnosis = await this.context.Diagnoses
                .Include(w => w.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(diagnosis);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(Int32? diagnosisId, Int32? patientId)
        {

            if (diagnosisId == null || patientId == null)
            {
                return NotFound();
            }

            var diagnosis = await context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.PatientId == patientId && m.DiagnosisId == diagnosisId);

            if (diagnosis == null)
            {
                return NotFound();
            }

            this.ViewBag.Diagnosis = diagnosis;

            return View(diagnosis);

        }

        // GET: Diagnoses/Create
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
            return this.View(new DiagnosisCreateAndEditModel());

        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, DiagnosisCreateAndEditModel model)
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

            var diagnoses = await context.Diagnoses.ToListAsync();
            var count = diagnoses.Count();

            if (this.ModelState.IsValid)
            {
                var diagnosis = new Diagnosis
                {
                    DiagnosisId = ++count,
                    PatientId = patient.Id,
                    Patient = patient,
                    Type = model.Type,
                    Complications = model.Complications,
                    Details = model.Details
                };

                this.context.Add(diagnosis);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            return this.View(model);

        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(Int32? diagnosisId, Int32? patientId)
        {

            if (diagnosisId == null || patientId == null)
            {
                return NotFound();
            }

            var diagnosis = await context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);

            if (diagnosis == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Diagnosis = diagnosis;

            var model = new DiagnosisCreateAndEditModel
            {
                Type = diagnosis.Type,
                Complications = diagnosis.Complications,
                Details = diagnosis.Details
            };

            return this.View(model);

        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? diagnosisId, Int32? patientId, DiagnosisCreateAndEditModel model)
        {
            if (diagnosisId == null || patientId == null)
            {
                return NotFound();
            }

            var diagnosis = await context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);

            if (diagnosis == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Diagnosis = diagnosis;

            if (this.ModelState.IsValid)
            {
                diagnosis.Type = model.Type;
                diagnosis.Complications = model.Complications;
                diagnosis.Details = model.Details;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = diagnosis.PatientId });
            }

            return this.View(model);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(Int32? diagnosisId, Int32? patientId)
        {
            if (diagnosisId == null || patientId == null)
            {
                return NotFound();
            }

            var diagnosis = await context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);

            if (diagnosis == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Diagnosis = diagnosis;

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? diagnosisId, Int32? patientId)
        {
            var diagnosis = await context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);

            this.ViewBag.Diagnosis = diagnosis;

            context.Diagnoses.Remove(diagnosis);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", new { patientId = diagnosis.PatientId });
        }

        private bool DiagnosisExists(int id)
        {
            return context.Diagnoses.Any(e => e.DiagnosisId == id);
        }
    }
}
