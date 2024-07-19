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
    public class BrandController : ControllerBase
    {
        private BrandBLL _BLL;

        public BrandController()
        {
            _BLL = new BrandBLL();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllBrands()
        {
            var brands = await _BLL.GetListBrand();
            return Ok(brands);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,ProductManager")]
        [HttpPost]
        public async Task<ActionResult> UpdateBrand([FromBody] dynamic request)
        {
            var brands = await _BLL.UpdateBrand(request);
            return Ok(brands);
        }
    }
}
