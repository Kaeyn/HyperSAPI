using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private CustomerBLL _BLL;

        public CustomerController()
        {
            _BLL = new CustomerBLL();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public ActionResult GetMyInfo()
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);
            var products = _BLL.GetCustomer(userID);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult GetListCustomer([FromBody] dynamic request)
        {
            var products = _BLL.GetListCustomer(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult UpdateCustomer([FromBody] dynamic request)
        {
            var products = _BLL.UpdateCustomer(request);          
            return Ok(products);
        }

        [HttpPost]
        public ActionResult UpdateShippingAddress([FromBody] dynamic request)
        {
            var result = _BLL.UpdateShippingAddress(request);

            return Ok(result);
        }
    }
}
