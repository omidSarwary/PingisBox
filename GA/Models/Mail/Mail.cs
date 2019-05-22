using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace GA.Models
{
    public class Mail : IMail
    {
        public void SendMail(MimeMessage message)
        {
            //configure and send the email
            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, false);
                client.Authenticate("email@outlook.com", "Password");
                client.Send(message);
                client.Disconnect(true);

            }
        }

        public void GenMail(string email,string title,string text)
        {
            var message = new MimeMessage();//instatiate message
            message.From.Add(new MailboxAddress("Pingiskåp", "gtg.se@outlook.com"));// from adress
            message.To.Add(new MailboxAddress(email));// to address
            message.Subject = title;//subject
            message.Body = new TextPart("plain")
            {
                Text = text
            };
           SendMail(message);//sends the mail
        }
    }
}
