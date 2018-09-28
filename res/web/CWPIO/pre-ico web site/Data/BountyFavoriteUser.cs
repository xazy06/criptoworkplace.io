using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Data
{
    public class BountyFavoriteUser
    {
        [JsonIgnore] public string UserId { get; set; }
        [JsonIgnore] public string FavoriteUserId { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser FavoriteUser { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
