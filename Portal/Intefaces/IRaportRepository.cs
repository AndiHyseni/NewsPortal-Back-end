using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Intefaces
{
  public   interface IRaportRepository
    {
       Task<DashboardViewModel> GetDashboardVIewModel();
        Task<FirstRaportViewModel> GetFirstRaportViewModel(FirstRaportViewModel model);
    }
}
