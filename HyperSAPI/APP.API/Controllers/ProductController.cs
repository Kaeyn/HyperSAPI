using APP.Bus.Repository.BLLs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductBLL _BLL;

        public ProductController()
        {
            _BLL = new ProductBLL();
        }

        // POST api/<ProductController>
        [HttpPost("GetAllProducts")]
        public ActionResult GetAllProducts()
        {
            var products = _BLL.GetAllProducts();
            return Ok(products);
        }

    }
}
