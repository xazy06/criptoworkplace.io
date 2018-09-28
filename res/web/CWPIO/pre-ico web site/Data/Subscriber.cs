using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Data
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailSend { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Unsubscribe { get; set; }
        public string Culture { get; set; }
    }
}
