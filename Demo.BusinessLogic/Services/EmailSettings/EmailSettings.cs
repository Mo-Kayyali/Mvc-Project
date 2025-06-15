using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mohamed.elkayyali88@gmail.com", "dnhgaswpzlzoncpv");
            client.Send("mohamed.elkayyali88@gmail.com",email.To,email.Subject,email.Body);
        }
    }
}
