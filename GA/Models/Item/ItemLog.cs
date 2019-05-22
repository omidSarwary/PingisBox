using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class ItemLog
    {
        public int Id { get; set; }
        [Display(Name = "Time")]
        public DateTime dateTime { get; set; }

        public string Message { get; set; }
    }
}
