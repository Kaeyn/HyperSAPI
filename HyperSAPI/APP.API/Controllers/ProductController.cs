using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using KendoNET.DynamicLinq;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductBLL _BLL;

        public ProductController()
        {
            _BLL = new ProductBLL();
        }

        // POST api/<ProductController>
        [HttpGet]
        public ActionResult HealthCheck()
        {
            return Ok("OK");
        }

        [HttpPost]
        public ActionResult GetProduct([FromBody] DTOProduct request)
        {
            var products = _BLL.GetProduct(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult GetListProduct([FromBody] DataSourceRequest options)
        {
            var products = _BLL.GetListProduct(options);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult GetListProductSale([FromBody] DataSourceRequest options)
        {
            var products = _BLL.GetListProductSale(options);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult GetListProductType()
        {
            var products = _BLL.GetListProductType();
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

    }
}
