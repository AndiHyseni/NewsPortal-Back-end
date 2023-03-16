using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Intefaces
{
    public interface IMailServices
    {
       
        
            Task<bool> SendEmailAsync(MailRequest mailRequest);
            Task SendWelcomeEmailAsync(WelcomeRequest request);
        
    }
}
