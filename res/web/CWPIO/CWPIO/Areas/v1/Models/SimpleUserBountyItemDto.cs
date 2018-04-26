using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.Areas.v1.Models
{
    public class SimpleUserBountyItemDto
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public int? ItemType { get; set; }
    }
}
