using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.DAL.Repository.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> GetListBill([FromBody] dynamic request)
        {
            var brands = await _BLL.GetListBill(request);
            if (brands.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult> GetBill(int CodeBill)
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);                   
            if (userID != null)
            {
                var brands = await _BLL.GetBill(CodeBill, userID);
                return Ok(brands);
            }
            return NotFound();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult> GetListCustomerBill()
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);
            if (userID != null)
            {
                var brands = await _BLL.GetListCustomerBill(userID);
                return Ok(brands);
            }
            
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Customer")]
        [HttpPost]
        public async Task<ActionResult> UpdateBill([FromBody] dynamic request)
        {
            var brands = await _BLL.UpdateBill(request);          
            return Ok(brands);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyCoupon(DTOApplyCouponRequest request)
        {
            var result = await _BLL.ApplyCoupon(request, true);
            return Ok(result);
        }
        
    }
}
