using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    { 
        private CartBLL _BLL;

        public CartController()
        {
            _BLL = new CartBLL();
        }


        [HttpPost]
        public ActionResult GetListCartProduct([FromBody] dynamic options)
        {
            var products = _BLL.GetListCartProduct(options);
            return Ok(products);
        }

        [HttpPost]
        public ActionResult GetCountInCart([FromBody] dynamic options)
        {
            var products = _BLL.GetCountInCart(options);
            return Ok(products);
        }
    }
}
