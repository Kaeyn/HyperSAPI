using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public ActionResult ProceedToPayment([FromBody] dynamic options)
        {
            var products = _BLL.ProceedToPayment(options);
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
