using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace pre_ico_web_site.Data
{
    public class ExchangeStatus
    {
        public string Id { get; set; }
        public string StartTx { get; set; }
        public string CurrentTx { get; set; }
        public bool IsEnded { get; set; }
        public bool IsFailed { get; set; }
        [JsonIgnore] public string CreatedByUserId { get; set; }
        [JsonIgnore] public DateTime DateCreated { get; set; }
        [JsonIgnore] public virtual ApplicationUser CreatedByUser { get; set; }
    }
}