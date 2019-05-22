using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GA.Helper;
using GA.Models;
using GA.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BoxController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly AppDbContext _appDbCotext;
        private readonly IChangeNotifierRepository _changeNotifierRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IHubContext<BoxHub> _boxHub;
        public BoxController(INotificationsRepository notificationsRepository, IItemRepository itemRepository, IChangeNotifierRepository changeNotifierRepository, IBoxRepository boxRepository, IStudentRepository studentRepository, AppDbContext appDbCotext, IHubContext<BoxHub> boxHub)
        {
            _notificationsRepository = notificationsRepository;
            _itemRepository = itemRepository;
            _boxHub = boxHub;
            _appDbCotext = appDbCotext;
            _changeNotifierRepository = changeNotifierRepository;
            _boxRepository = boxRepository;
            _studentRepository = studentRepository;
        }
                                         
        // POST: api/Box
        // gets the user code and rfid and registers rfid to the user by code
        [HttpPost("UpdateBoxStatus")]
        public async Task UpdateBoxStatus(BoxStats boxstats)
        {
            
          
            
            var student = _studentRepository.GetStudentByRFID(boxstats.Rfid);
            var log = new BoxLog
            {
                dateTime = DateTime.Now
            };
            var Box = _boxRepository.GetBox();
            Box.IsOpen = boxstats.door;
            Box.StudentOppend = student;
            Box.StudentId = student.Id;
            if (boxstats.door)
            {
                if (student != null)
                    log.Message = "Box Door Opend By " + student.FullName;
                else
                {
                    log.Message = "Warning! Box Door opend by unknown User";
                    //activate warning
                }


            }
            if (!boxstats.door)
            {
                log.Message = "Box Door Closed";

            }
            _boxRepository.AddLog(log);
         
            await _boxHub.Clients.All.SendAsync("doorStats", Box.StudentOppend.email,Box.IsOpen, DateTime.Now.ToString("HH:mm:ss"));
            await _notificationsRepository.AddNotificationAsync(log.Message);
          

        }


    }
}
