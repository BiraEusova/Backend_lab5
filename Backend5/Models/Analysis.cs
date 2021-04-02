using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Analysis
    {
        public Int32 AnalysisId { get; set; }

        public Int32 PatientId { get; set; }

        public Patient Patient { get; set; }

        public Int32? LabId { get; set; }

        public Lab Lab { get; set; }

        [Required]
        [MaxLength(100)]
        public String Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public String Status { get; set; }
    }
}
