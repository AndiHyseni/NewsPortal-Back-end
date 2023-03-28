using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Boolean IsFeatured { get; set; }
        public Boolean IsDeleted { get; set; }
        public int? NumberOfClicks { get; set; }
        public string Content { get; set; }

        public string Image { get; set; }
        public string Video { get; set; }
        public  Category Category { get; set; }
        public DateTime ExpireDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime UpdatedOnDate { get; set; }
        public string CreatedBy { get; set; }
        public string tags { get;set;}
        public IEnumerable<Watched> views { get; set; }
        

    }
}
