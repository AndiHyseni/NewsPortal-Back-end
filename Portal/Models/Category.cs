using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Category

    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [MinLength(20,ErrorMessage ="there need to be more than 20 characters")]
        public string Description { get; set; }
      
        [Required]
        public Boolean ShowOnline { get; set; 
        }
    



    }
}
