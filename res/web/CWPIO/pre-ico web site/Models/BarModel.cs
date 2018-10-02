using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Models
{
    public class BarModel
    {
        public int Sold { get; set; }
        public int Cap { get; set; }
        public float Percent => Sold * 100 / Cap;
    }
}
