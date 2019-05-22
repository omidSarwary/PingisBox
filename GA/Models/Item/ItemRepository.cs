using GA.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class ItemRepository : IItemRepository
    {
        private readonly IHubContext<BoxHub> _boxHub;
        private readonly AppDbContext _appDbCotext;
        public ItemRepository(AppDbContext appDbCotext, IHubContext<BoxHub> boxHub)
        {
            _boxHub = boxHub;
            _appDbCotext = appDbCotext;
        }

        public void AddItem()
        {
            var box = new Item
            {
                IsInBox = true,              

            };

            _appDbCotext.Items.Add(box);
            _appDbCotext.SaveChanges();
        }

        public void AddLog(ItemLog log)
        {
            _appDbCotext.itemLogs.Add(log);
            _appDbCotext.SaveChanges();
        }

        public IEnumerable<ItemLog> GetAllLogs()
        {
            return _appDbCotext.itemLogs;
        }

        public Item GetItemByRFID(ulong Id)
        {
            return _appDbCotext.Items.FirstOrDefault(p => p.RFID == Id);
        }
        public Item GetItemById(int Id)
        {
            return _appDbCotext.Items.FirstOrDefault(p => p.Id == Id);
        }

        public ItemLog GetLastLog()
        {
            return _appDbCotext.itemLogs.Last();
        }


        public async Task<ItemCount> GetCount()
        {
            var date = DateTime.Now.ToString("dd/MM-yy");
            var _count = _appDbCotext.itemCount.Last();
            if (_count.Time != date || _count == null)
            {
                var count = new ItemCount
                {
                    Time = DateTime.Now.ToString("dd/MM-yy"),
                    TimesBorrowed = 0
                };
                _appDbCotext.itemCount.Add(count);
                await _appDbCotext.SaveChangesAsync();
                return count;
            }
            else
            return  _appDbCotext.itemCount.Last();
        }

         public async Task count()
        {
            var date=DateTime.Now.ToString("dd/MM-yy");
            var _count = _appDbCotext.itemCount.Last();

                if(_count.Time!=date || _count==null)
                {
                    var count = new ItemCount
                    {
                        Time = DateTime.Now.ToString("dd/MM-yy"),
                        TimesBorrowed = 1
                    };
                _appDbCotext.itemCount.Add(count);
                await _boxHub.Clients.All.SendAsync("TimesBorrowedToday", count.TimesBorrowed, count.Time, DateTime.Now.ToString("HH:mm:ss"));
            }
                else
                {
                    _count.TimesBorrowed = _count.TimesBorrowed + 1;
                await _boxHub.Clients.All.SendAsync("TimesBorrowedToday", _count.TimesBorrowed, _count.Time, DateTime.Now.ToString("HH:mm:ss"));
            }
            await _appDbCotext.SaveChangesAsync();
           
            

        }

        public IEnumerable<ItemCount> GetAllCounts()
        {
            return _appDbCotext.itemCount;
        }


        public IEnumerable<ItemCount> GetLastCounts(int mm)
        {
            List<ItemCount> counts = _appDbCotext.itemCount.OrderByDescending(p => p.Id).Take(mm).ToList();
            return counts;
        }


    }
}
