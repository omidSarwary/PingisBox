using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Display(Name = "Racket RFID")]
        public ulong RFID { get; set; }
        [Display(Name = "Racket Status")]
        public bool IsInBox { get; set; }
        [Display(Name = "User Borrowed the racket")]
        public string StudentBorrowed { get; set; }
        public int StudentId { get; set; }
        [BindNever]
        public bool Changed { get; set; }
    }
}
