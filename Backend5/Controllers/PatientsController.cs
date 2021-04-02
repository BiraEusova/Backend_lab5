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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PatientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return this.View(new PatientCreateAndEditModel());
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCreateAndEditModel model)
        {
            if (this.ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Name = model.Name,
                    Address = model.Address,
                    BirthDay = model.BirthDay,
                    Gender = model.Gender
                };

                this.context.Patients.Add(patient);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);

        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return this.NotFound();
            }

            var model = new PatientCreateAndEditModel
            {
                Name = patient.Name,
                Address = patient.Address,
                BirthDay = patient.BirthDay,
                Gender = patient.Gender
            };

            return this.View(model);

        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PatientCreateAndEditModel model)
        {

            if (id == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                patient.Name = model.Name;
                patient.Address = model.Address;
                patient.BirthDay = model.BirthDay;
                patient.Gender = model.Gender;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);

        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var patient = await context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            context.Patients.Remove(patient);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PatientExists(int id)
        {
            return context.Patients.Any(e => e.Id == id);
        }
    }
}
