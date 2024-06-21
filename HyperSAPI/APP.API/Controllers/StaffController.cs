using APP.Bus.Repository.BLLs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private StaffBLL _BLL;

        public StaffController()
        {
            _BLL = new StaffBLL();
        }

        [HttpPost]
        public ActionResult GetListStaff([FromBody] dynamic request)
        {
            var products = _BLL.GetListStaff(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult UpdateStaff([FromBody] dynamic request)
        {
            var products = _BLL.UpdateStaff(request);
            return Ok(products);
        }
    }
}
