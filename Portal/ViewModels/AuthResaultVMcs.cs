using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class AuthResaultVMcs
    {
        public string token { get; set; }

        public string refreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }


    }
}
