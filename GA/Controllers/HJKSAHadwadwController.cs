using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using GA.Hubs;
using GA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;

namespace GA.Controllers
{
    [Authorize]
    public class HJKSAHadwadwController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IHubContext<BoxHub> _boxHub;
        private readonly IBoxRepository _boxRepository;
        private readonly IItemRepository _itemRepository;
        private readonly INotificationsRepository _notificationsRepository;

        public HJKSAHadwadwController(INotificationsRepository notificationsRepository, IStudentRepository studentRepository,  IBoxRepository boxRepository, IItemRepository itemRepository,  IHubContext<BoxHub> boxHub)
        {
            _notificationsRepository = notificationsRepository;
            _boxHub = boxHub;
            _boxRepository = boxRepository;
            _itemRepository = itemRepository;
            _studentRepository = studentRepository;
        }
        // GET: Admin
        public IActionResult Index()
        {
            ViewBag.asd = "Admin";
            return View();
        }

     
        public ActionResult notification()
        {
            return PartialView("_Notifications", _notificationsRepository.GetLast15Notifications());
        }
    }




}