using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class CategoryViewModel
    {
        
        public int CategoryId { get; set; }
      
        public string Name { get; set; }
        
        public string Description { get; set; }

       
        public string ShowOnline
        {
            get; set;
        }
    }
}
