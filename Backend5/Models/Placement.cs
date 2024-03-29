﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Placement
    {
        [Required]
        [MaxLength(10)]
        public Int32 Bed { get; set; }

        public Int32 WardId { get; set; }

        public Ward Ward { get; set; }

        public Int32 PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
