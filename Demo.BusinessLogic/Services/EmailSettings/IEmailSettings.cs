using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.EmailSettings
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
