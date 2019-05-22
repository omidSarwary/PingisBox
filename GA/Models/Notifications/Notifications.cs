using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class Notifications
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }
        [Display(Name = "Notification")]
        public string Message { get; set; }
        public bool New { get; set; }
    }
}
    