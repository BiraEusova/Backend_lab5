﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Patient
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Address { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        public String Gender { get; set; }

        public ICollection<DoctorPatient> Doctors { get; set; }

        public ICollection<Placement> Placements { get; set; }

        //public ICollection<Analysis> Analyzes { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; }

    }
}
