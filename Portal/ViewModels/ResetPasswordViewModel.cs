using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 16 characters")]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
