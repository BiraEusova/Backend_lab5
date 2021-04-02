using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class AnalysisCreateAndEditModel
    {
        [Required]
        [MaxLength(100)]
        public String Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Int32 LabId { get; set; }

        public String Status { get; set; }

    }
}
