using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public interface IItemRepository 
    {
        void AddItem();//one time
        Item GetItemByRFID(ulong Id);
        Item GetItemById(int Id);
        IEnumerable<ItemLog> GetAllLogs();
        void AddLog(ItemLog log);
        ItemLog GetLastLog();
        Task count();
        Task<ItemCount> GetCount();
        IEnumerable<ItemCount> GetAllCounts();
        IEnumerable<ItemCount> GetLastCounts(int mm);

    }
}
