using GA.Helper;
using GA.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly AppDbContext _appDbCotext;
        private readonly IHubContext<BoxHub> _boxHub;
        private readonly IChangeNotifierRepository _changeNotifier;
        public NotificationsRepository (AppDbContext appDbCotext, IHubContext<BoxHub> boxHub, IChangeNotifierRepository changeNotifier)
        {
            _appDbCotext = appDbCotext;
            _boxHub = boxHub;
            _changeNotifier = changeNotifier;
        }

        public async Task AddNotificationAsync(string message)
        {
            await _changeNotifier.NewChanges();          
            var notifications = new Notifications
            {
                Time = DateTime.Now,
                Message = message
            };
            _appDbCotext.notifications.Add(notifications);
            _appDbCotext.SaveChanges();
            await _boxHub.Clients.All.SendAsync("AddNotification", message, DateTime.Now.ToString("dddd, dd MMMM, HH:MM"));
        }

        public void DeleteOldNotifications()
        {
            _appDbCotext.notifications.RemoveRange(_appDbCotext.notifications.Where(x => x.Time < DateTime.Now.AddMonths(-1)));
         
        }

        public IEnumerable<Notifications> GetAllNotifications()
        {
            return _appDbCotext.notifications;
        }

        public IEnumerable<Notifications> GetLast15Notifications()
        {
            List <Notifications> notifications = _appDbCotext.notifications.OrderBy(p => p.Time).Take(15).ToList();
            return notifications;
       
        }
    }
}
    