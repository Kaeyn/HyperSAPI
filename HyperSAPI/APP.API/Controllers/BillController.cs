using APP.Bus.Repository.BLLs;
using APP.DAL.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private BillBLL _BLL;

        public BillController()
        {
            _BLL = new BillBLL();
        }

        [HttpPost]
        public ActionResult GetListBill([FromBody] dynamic request)
        {
            var brands = _BLL.GetListBill(request);
            if (brands.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult GetBill(int CodeBill)
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);                   
            if (userID != null)
            {
                var brands = _BLL.GetBill(CodeBill, userID);
                if (brands.ObjectReturn?.Data == null)
                {
                    return NotFound();
                }
                return Ok(brands);
            }
            return NotFound();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult GetListCustomerBill()
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);
            if (userID != null)
            {
                var brands = _BLL.GetListCustomerBill(userID);
                if (brands.ObjectReturn?.Data == null)
                {
                    return NotFound();
                }
                return Ok(brands);
            }
            
            return NotFound();
        }

        [HttpPost]
        public ActionResult UpdateBill([FromBody] dynamic request)
        {
            var brands = _BLL.UpdateBill(request);          
            return Ok(brands);
        }

        
    }
}
