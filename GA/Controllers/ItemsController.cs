using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GA.Helper;
using Microsoft.AspNetCore.SignalR;
using GA.Hubs;
using Hangfire;
using MimeKit;

namespace GA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly AppDbContext _appDbCotext;
        private readonly IMail _mail;
        private readonly IChangeNotifierRepository _changeNotifierRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IHubContext<BoxHub> _boxHub;
        public ItemsController(INotificationsRepository notificationsRepository, IMail mail, IItemRepository itemRepository, IChangeNotifierRepository changeNotifierRepository, IBoxRepository boxRepository, IStudentRepository studentRepository, AppDbContext appDbCotext, IHubContext<BoxHub> boxHub)
        {
            _notificationsRepository = notificationsRepository;
            _itemRepository = itemRepository;
            _boxHub = boxHub;
            _mail = mail;
            _appDbCotext = appDbCotext;
            _changeNotifierRepository = changeNotifierRepository;
            _boxRepository = boxRepository;
            _studentRepository = studentRepository;
        }

        // POST: api/Box
        // gets the user code and rfid and registers rfid to the user by code
        [HttpPost("UpdateItemStatus")]
        public async Task UpdateItemStatus(ItemStats itemStats)
        {
            var student = _studentRepository.GetStudentByRFID(itemStats.StudentRFID);
            var log = new ItemLog
            {
                dateTime = DateTime.Now
            };
            var item = _itemRepository.GetItemById(itemStats.Id);
            item.IsInBox = itemStats.IsInBox;
            item.StudentBorrowed = student.FullName;
            item.StudentId = student.Id;
            if (itemStats.IsInBox)
            {
                if (student != null)
                    log.Message = "Racket No " + item.Id + " Is returned by: " + student.email;
                else
                {
                    log.Message = "Warning! item is returned but with unkown User";
                    //activate warning
                }


            }
            if (!itemStats.IsInBox)
            {
                log.Message = "Racket No " + item.Id + " Is Borrowed by: " + student.FullName;
                await  _itemRepository.count();

            }
            _itemRepository.AddLog(log);
            if (item.Id == 1)
            {

                await _boxHub.Clients.All.SendAsync("Item1", item.StudentBorrowed, item.IsInBox, DateTime.Now.ToString("HH:mm:ss"));
                await _notificationsRepository.AddNotificationAsync(log.Message);
            }
            if (item.Id == 2)
            {
                await _boxHub.Clients.All.SendAsync("Item2", item.StudentBorrowed, item.IsInBox, DateTime.Now.ToString("HH:mm:ss"));
                await _notificationsRepository.AddNotificationAsync(log.Message);
            }
            _mail.GenMail(
                        student.email, "You borrowed a Racket from pingisBox",
                        "You borrowed A racket with id: " + item.RFID + " from pingisBox \nBorrowing time:"
                        + DateTime.Now.ToString("HH:mm:ss") + " You should return it letast at"
                        + DateTime.Now.AddHours(1).ToString("HH:mm:ss"));
            if (!item.IsInBox)
            {
                BackgroundJob.Schedule(() =>

                Reminder(item.Id, student.email, "Reminder From pingisBox",
                       "You borrowed A racket with id: " + item.RFID + " from pingisBox \nBorrowing time:"
                       + DateTime.Now.ToString("HH:mm:ss") + "\nPlease return the racket as soon as possible"
                      ), TimeSpan.FromMinutes(60)
                      
                      
                      );
            }
        }


        public void Reminder(int id, string email,string subject,string text)
        {
            var item = _itemRepository.GetItemById(id);
            if (!item.IsInBox)
            {
                _mail.GenMail(email,subject,text);
            }          

        }

        [HttpGet("GetItemStatus")]
        public  ItemStatsPie GetItemStatus()
        {            
            var item1 = _itemRepository.GetItemById(1);
            var item2 = _itemRepository.GetItemById(2);  
            var stats = new ItemStatsPie { ItemOne = item1.IsInBox,ItemTwo = item2.IsInBox };
            return stats;
        }
    }
}
