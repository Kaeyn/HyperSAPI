using APP.Bus.Repository.BLLs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public ActionResult UpdateCoupon([FromBody] dynamic request)
        {
            var products = _BLL.UpdateCoupon(request);
            return Ok(products);
        }
    }
}
