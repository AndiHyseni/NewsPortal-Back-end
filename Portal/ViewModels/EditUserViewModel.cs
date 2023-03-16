using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string role { get; set; }
        
    }
}
