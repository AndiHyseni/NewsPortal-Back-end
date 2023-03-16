using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Watched
    {
        [Key]
        public int watchId { get; set; }
        public string userId { get; set; }
        public string FingerPrintId { get; set; }
        public int newsId { get; set; }
        public News News { get; set; }

        public IdentityUser User { get; set; }
        public DateTime watchedOn { get; set; }


    }
}
