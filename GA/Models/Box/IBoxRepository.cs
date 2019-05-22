using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public interface IBoxRepository
    {
        void AddBox();//onetime
        Box GetBox();
        IEnumerable<BoxLog> GetAllLogs();
        void AddLog(BoxLog log);
        BoxLog GetLastLog();

    }
}
