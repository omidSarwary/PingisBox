using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public interface INotificationsRepository
    {
        IEnumerable<Notifications> GetLast15Notifications();
        IEnumerable<Notifications> GetAllNotifications();
        Task AddNotificationAsync(string notifications);
        void DeleteOldNotifications();
        
    }
}
    