using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class FirstRaportViewModel
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string mainElement { get; set; }
        public string whatToShow { get; set; }
        public List<FirstRaportResultViewModel> result { get; set; }
    }
}
