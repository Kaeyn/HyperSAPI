using APP.Bus.Repository.BLLs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private CouponBLL _BLL;

        public CouponController()
        {
            _BLL = new CouponBLL();
        }

        [HttpPost]
        public ActionResult GetListCoupon([FromBody] dynamic request)
        {
            var products = _BLL.GetListCoupon(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult UpdateCoupon([FromBody] dynamic request)
        {
            var products = _BLL.UpdateCoupon(request);
            return Ok(products);
        }
    }
}
