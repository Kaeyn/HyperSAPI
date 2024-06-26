using APP.Bus.Repository.BLLs;
using APP.DAL.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private BillBLL _BLL;

        public BillController()
        {
            _BLL = new BillBLL();
        }

        [HttpPost]
        public ActionResult GetListBill([FromBody] dynamic request)
        {
            var brands = _BLL.GetListBill(request);
            if (brands.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpPost]
        public ActionResult UpdateBill([FromBody] dynamic request)
        {
            var brands = _BLL.UpdateBill(request);          
            return Ok(brands);
        }
    }
}
