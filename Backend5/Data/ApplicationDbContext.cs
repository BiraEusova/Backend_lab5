﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend5.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Hospital> Hospitals { get; set; }              

        public DbSet<Lab> Labs { get; set; }                        

        public DbSet<Doctor> Doctors { get; set; }                  

        public DbSet<Patient> Patients { get; set; }                

        public DbSet<Placement> Placements { get; set; }            

        public DbSet<Ward> Wards { get; set; }                      

        public DbSet<Analysis> Analyses { get; set; }               

        public DbSet<Diagnosis> Diagnoses { get; set; }             

        public DbSet<WardStaff> WardStafs { get; set; }               

        public DbSet<HospitalPhone> HospitalPhones { get; set; }       

        public DbSet<LabPhone> LabPhones { get; set; }              

        public DbSet<HospitalLab> HospitalLabs { get; set; }        

        public DbSet<DoctorPatient> DoctorPatients { get; set; }    

        public DbSet<HospitalDoctor> HospitalDoctors { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HospitalPhone>()
                .HasKey(x => new { x.HospitalId, x.PhoneId });      // композитный ключ для HospitalPhone (ID)

            modelBuilder.Entity<LabPhone>()
                .HasKey(x => new { x.LabId, x.PhoneId });           // композитный ключ для LabPhone (ID)

            modelBuilder.Entity<WardStaff>()
                .HasKey(x => new { x.WardStaffId, x.WardId });            // композитный ключ для WardStaff (ID)

            modelBuilder.Entity<Diagnosis>()
                .HasKey(x => new { x.DiagnosisId, x.PatientId });            // композитный ключ для Diagnosis (ID)

            modelBuilder.Entity<Analysis>()
                .HasKey(x => new { x.AnalysisId, x.PatientId });            // композитный ключ для Analysis (ID)

            modelBuilder.Entity<Placement>()
               .HasKey(x => new { x.WardId, x.PatientId });            // композитный ключ для Placement (ID)

            modelBuilder.Entity<HospitalLab>()
                .HasKey(x => new { x.HospitalId, x.LabId });        // композитный ключ для HospitalLab (M to M)

            modelBuilder.Entity<DoctorPatient>()
                .HasKey(x => new { x.DoctorId, x.PatientId });      // композитный ключ для DoctorPatient (M to M)

            modelBuilder.Entity<HospitalDoctor>()
                .HasKey(x => new { x.HospitalId, x.DoctorId });     // композитный ключ для HospitalDoctor (M to M)

        }
    }
}
