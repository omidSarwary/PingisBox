using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class ItemCount
    {
        public int Id { get; set; }
        [Display(Name = "Times Borrowed")]
        public int TimesBorrowed { get; set; }
        [Display(Name = "Day")]
        public string Time { get; set; }

    }
}
