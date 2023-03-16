
using Microsoft.AspNetCore.Mvc;
using Portal.Intefaces;
using Portal.Models;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class NewsConfigController : ControllerBase
    {
        private readonly INewsConfig _newsConfig;
     public NewsConfigController(INewsConfig newsConfig)
        {
            _newsConfig = newsConfig;
        }

        [HttpGet]
        public async Task<ActionResult<NewsConfig>> GetNewsConfig()
        {
           var editrow=await _newsConfig.GetConfigRow();
            if(editrow==null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(editrow);
        }
        [HttpPost]
        public async Task<ActionResult> EditNewsConfigRow(NewsConfig newsConfig)
        {

          var task= _newsConfig.EditConfigRow(newsConfig);
            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");


        }
    }
}
