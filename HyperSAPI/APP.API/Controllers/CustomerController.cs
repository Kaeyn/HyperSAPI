using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public ActionResult GetCustomer(DTOCustomer request)
        {
            var products = _BLL.GetCustomer(request);
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
    }
}
