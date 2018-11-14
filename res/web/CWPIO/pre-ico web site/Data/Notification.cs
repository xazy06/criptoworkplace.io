using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Data
{
    public class Notification
    {
        public string Id { get; set; }
        public bool IsRead { get { return DateReaded.HasValue; } }
        public NotificationType Type { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateReaded { get; set; }
        public string ToUserId { get; set; }
        public virtual ApplicationUser ToUser { get; set; }
    }

    public enum NotificationType:int
    {
        Simple,
        Important,
        Personal,
        Announcement
    }
}
