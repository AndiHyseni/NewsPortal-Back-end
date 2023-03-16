using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Reaction
    {
        [Key]
        public int reactionId { get; set; }
       
        public string userId { get; set; }

        public int newsId { get; set; }
        public int reaction { get; set; }
        public IdentityUser user { get; set; }
        public News news { get; set; }


    }
}
