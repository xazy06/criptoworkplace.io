using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Areas.v1.Models
{
    public class SimpleBountyItemDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public bool NeedToApprove { get; set; }
    }
}
