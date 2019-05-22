using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class BoxRepository : IBoxRepository
    {
        private readonly AppDbContext _appDbCotext;
        public BoxRepository(AppDbContext appDbCotext)
        {
            _appDbCotext = appDbCotext;
        }
        public void AddLog(BoxLog log)
        {
            _appDbCotext.boxLogs.Add(log);
            _appDbCotext.SaveChanges();
        }
        public void AddBox()//one time
        {
            var box = new Box
            {
                IsOpen = false,
                StudentOppend = null

            };

            _appDbCotext.Box.Add(box);
            _appDbCotext.SaveChanges();
        }

        public IEnumerable<BoxLog> GetAllLogs()
        {
            return _appDbCotext.boxLogs;
        }

        public Box GetBox()
        {
            return _appDbCotext.Box.FirstOrDefault();
        }

        public BoxLog GetLastLog()
        {
            return _appDbCotext.boxLogs.Last();
        }
    }
}
