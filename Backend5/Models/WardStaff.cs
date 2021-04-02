using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class WardStaff
    {
        [Required]
        [MaxLength(20)]
        public String Name { get; set; }

        [MaxLength(20)]
        public String Position { get; set; }

        public Int32 WardStaffId { get; set; }

        public Int32 WardId { get; set; }

        public Ward Ward { get; set; }        
    }
}
