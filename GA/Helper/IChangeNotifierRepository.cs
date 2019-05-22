using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Helper
{
    public interface IChangeNotifierRepository
    {
      ChangeNotifier GetChangeNotifier() ;
       Task NewChanges();
        Task Reset();
        //void Addchange();
    }
}
