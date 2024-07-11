using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
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
        public async Task<ActionResult> GetMyInfo()
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);
            var products = await _BLL.GetCustomer(userID);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> GetListCustomer([FromBody] dynamic request)
        {
            var products = await _BLL.GetListCustomer(request);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCustomer([FromBody] dynamic request)
        {
            var products = await _BLL.UpdateCustomer(request);          
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult> UpdateShippingAddress([FromBody] dynamic request)
        {
            var result = await _BLL.UpdateShippingAddress(request);

            return Ok(result);
        }
    }
}
