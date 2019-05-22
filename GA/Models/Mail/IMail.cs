using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public interface IMail
    {
        void SendMail(MimeMessage message);
        void GenMail(string email, string title, string text);
    }
}
