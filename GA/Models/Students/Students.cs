 using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class Students
    {
       
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Your email is required")]
        [DataType(DataType.EmailAddress)]
       // [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@gtg\.se|@gtc\.com)$", ErrorMessage = "Only gtg.se and gtc.com emails are allowed!")]
        [Display(Name = "Email")]
        public string email { get; set; }
       
        [Display(Name = "Activation Code")]
        public int code { get; set; }
      
        public ulong RFID { get; set; }
    
        [Display(Name = "Borrowed Any Item")]
        public bool IsBorrowed { get; set; }
       
        [Display(Name = "Item Borrowed RFID")]
        public ulong  BorrowedItem  { get; set; }
        
        [Display(Name = "Item Borrowed RFID")]
        public ulong BorrowedItem1 { get; set; }
    
        [Display(Name = "Borrowing Time")]
        public DateTime BorrowedTime { get; set; }
     
        [Display(Name = "Handed Back the borowed Item")]
        public bool HandedBack { get; set; }
       
        [Display(Name = "Exceeded Allowed borrowing time")]
        public bool IsOverTime { get; set; }
        
        [Display(Name = "Users full name")]
        public string FullName { get; set; }
    }
}
