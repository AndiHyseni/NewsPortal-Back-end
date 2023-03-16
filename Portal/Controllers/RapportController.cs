using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Intefaces;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RapportController : ControllerBase
    {
        private IRaportRepository _raportRepository;
        public RapportController(IRaportRepository raportRepository)
        {
            _raportRepository = raportRepository;
        }

        [HttpGet]
      public async Task<ActionResult<DashboardViewModel>> GetDashboarViewModelAsync()
        {
            var Model =await  _raportRepository.GetDashboardVIewModel();

            if (Model == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(Model);
        }
        [HttpPost]
        public async Task<ActionResult<FirstRaportViewModel>> GetFirstRaport(FirstRaportViewModel model)
        {
            var newmodel = await _raportRepository.GetFirstRaportViewModel(model);

            if (newmodel == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(newmodel);
        }
    }
}
