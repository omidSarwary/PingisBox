using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class Box
    {
        public int Id { get; set; }
        [Display(Name = "Door Status")]
        public bool IsOpen { get; set; }
        [Display(Name = "User Oppened the door")]
        public Students StudentOppend { get; set; }
        public int StudentId { get; set; }
        [BindNever]
        public bool Changed { get; set; }
    }
}
