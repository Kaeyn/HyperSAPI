using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Product;
using KendoNET.DynamicLinq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<ActionResult> GetProduct([FromBody] DTOProduct request)
        {
            var products = await _BLL.GetProduct(request);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> GetListProduct([FromBody] dynamic options)
        {
            var products = await _BLL.GetListProduct(options);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> GetListProductSale([FromBody] dynamic options)
        {
            var products = await _BLL.GetListProductSale(options);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> GetListProductType()
        {
            var products = await _BLL.GetListProductType();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddProductToCart(DTOAddToCart request)
        {
            var products = await _BLL.AddProductToCart(request);
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateProduct([FromBody] dynamic request)
        {
            var products = await _BLL.UpdateProduct(request);
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateProductType([FromBody] dynamic request)
        {
            var products = await _BLL.UpdateProductType(request);
            return Ok(products);
        }
    }
}
