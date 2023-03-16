using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class NewsConfig
    {
        [Key]
        public int NewsConfigId { get; set; }
        public string HeaderLogo { get; set; }
        public string FooterLogo { get; set; }
        public Boolean ShowFeatured { get; set; }
        public Boolean ShowMostWached { get; set; }
        public Boolean ShowRelatedNews { get; set; }

    }
}
