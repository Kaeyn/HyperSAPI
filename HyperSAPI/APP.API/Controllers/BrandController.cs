using APP.Bus.Repository.BLLs;
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
        public ActionResult GetAllBrands()
        {
            var brands = _BLL.GetAllBrands();
            return Ok(brands);
        }
    }
}
