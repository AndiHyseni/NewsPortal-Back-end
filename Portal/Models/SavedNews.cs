using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class SavedNews
    {
        [Key]
        public int Id { get; set; }
        public int NewsId { get; set; }

        public string UserId { get; set; }
        public News News { get; set; }
        public IdentityUser User { get; set; }



    }
}
