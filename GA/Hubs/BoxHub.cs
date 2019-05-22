using GA.Helper;
using GA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hangfire;

namespace GA.Hubs
{
    [Authorize]
    public class BoxHub : Hub
    {
        private readonly IChangeNotifierRepository _changeNotifier;
        private readonly IStudentRepository _studentRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IItemRepository _itemRepository;
        private readonly AppDbContext _appDbCotext;
        private readonly INotificationsRepository _notificationsRepository;
        public BoxHub(IChangeNotifierRepository changeNotifier, INotificationsRepository notificationsRepository, IStudentRepository studentRepository, AppDbContext appDbCotext, IBoxRepository boxRepository, IItemRepository itemRepository)
        {
            _notificationsRepository = notificationsRepository;
            _studentRepository = studentRepository;
            _boxRepository = boxRepository;
            _itemRepository = itemRepository;
            _appDbCotext = appDbCotext;
            _changeNotifier = changeNotifier;
        }

        public async Task GetUpdate()
        {
            Box box = _boxRepository.GetBox();
            Item item1 = _itemRepository.GetItemById(1);
            Item item2 = _itemRepository.GetItemById(2);
            Students students1 = _studentRepository.GetStudentById(box.StudentId);
            Students students2 = _studentRepository.GetStudentById(item1.StudentId);
            Students students3 = _studentRepository.GetStudentById(item2.StudentId);
            ItemCount itemCount =await _itemRepository.GetCount();
            await Clients.Caller.SendAsync("doorStats", students1.FullName,box.IsOpen, DateTime.Now.ToString("HH:mm:ss"));
            await Clients.Caller.SendAsync("Item1", students2.FullName, item1.IsInBox, DateTime.Now.ToString("HH:mm:ss"));
            await Clients.Caller.SendAsync("Item2", students3.FullName, item2.IsInBox, DateTime.Now.ToString("HH:mm:ss"));
            await Clients.Caller.SendAsync("TimesBorrowedToday", itemCount.TimesBorrowed, itemCount.Time, DateTime.Now.ToString("HH:mm:ss"));
        }

        public async Task NotificationUpdate()
        {
            var NewChanges = _changeNotifier.GetChangeNotifier().changed;
            var notifications = _notificationsRepository.GetLast15Notifications();
            string json = JsonConvert.SerializeObject(notifications);
            await Clients.Caller.SendAsync("AddAllNotifications", json,NewChanges);
        }

        public async Task Reset()
        {
           await _changeNotifier.Reset();
           
        }
    }
}

