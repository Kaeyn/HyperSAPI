using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,BillManager,EventManager")]
        [HttpPost]
        public async Task<ActionResult> GetListCoupon([FromBody] dynamic request)
        {
            var products = await _BLL.GetListCoupon(request);
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,EventManager")]
        [HttpPost]
        public async Task<ActionResult> UpdateCoupon([FromBody] dynamic request)
        {
            var products = await _BLL.UpdateCoupon(request);
            return Ok(products);
        }
    }
}
