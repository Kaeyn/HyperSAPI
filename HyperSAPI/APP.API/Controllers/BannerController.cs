using APP.Bus.Repository.BLLs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private BannerBLL _BLL;

        public BannerController()
        {
            _BLL = new BannerBLL();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public ActionResult GetListBanner([FromBody] dynamic request)
        {
            var products = _BLL.GetListBanner(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public ActionResult UpdateBanner([FromBody] dynamic request)
        {
            var products = _BLL.UpdateBanner(request);
            return Ok(products);
        }


    }
}
