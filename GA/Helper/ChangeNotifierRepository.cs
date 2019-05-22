using GA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Helper
{
    public class ChangeNotifierRepository : IChangeNotifierRepository
    {
        private readonly AppDbContext _appDbCotext;
        public ChangeNotifierRepository(AppDbContext appDbCotext)
        {
            _appDbCotext = appDbCotext;
        }
        public ChangeNotifier GetChangeNotifier()
        {
            return _appDbCotext.changeNotifier.FirstOrDefault(p => p.Id == 1);
        }
        public async Task NewChanges()
        {
            var changes = _appDbCotext.changeNotifier.FirstOrDefault(p => p.Id == 1);
            changes.changed = true;
            await _appDbCotext.SaveChangesAsync();
        }
        public async Task Reset()
        {
            var changes = _appDbCotext.changeNotifier.FirstOrDefault(p => p.Id == 1);
            changes.changed = false;
            await _appDbCotext.SaveChangesAsync();
        }

        //public void Addchange()//one time
        //{
        //    var changer = new ChangeNotifier
        //    {
        //        changed = false,


        //    };

        //    _appDbCotext.changeNotifier.Add(changer);
        //    _appDbCotext.SaveChanges();
        //}
    }
}
