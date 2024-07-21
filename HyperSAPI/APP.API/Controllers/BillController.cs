using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.Services;
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
        private CartBillBLL cartBillBLL;
        public BillController()
        {
            _BLL = new BillBLL();
            cartBillBLL = new CartBillBLL();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,BillManager")]
        [HttpPost]
        public async Task<ActionResult> GetListBill([FromBody] dynamic request)
        {
            var brands = await _BLL.GetListBill(request);
            return Ok(brands);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult> GetBill([FromBody] int CodeBill)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,BillManager,Customer")]
        [HttpPost]
        public async Task<ActionResult> UpdateBill([FromBody] dynamic request)
        {
            var brands = await _BLL.UpdateBill(request);          
            return Ok(brands);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,BillManager,Customer")]
        [HttpPost]
        public async Task<ActionResult> UpdateBillStaff([FromBody] dynamic request)
        {
            var brands = await _BLL.UpdateBillStaff(request);
            return Ok(brands);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> GetBillAnalystic([FromBody] dynamic request)
        {
            var result = await _BLL.GetBillAnalystic();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        [HttpPost]
        public async Task<ActionResult> GetMonthYearAnalystic([FromBody] dynamic request)
        {
            var result = await _BLL.GetMonthYearAnalystic(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyCoupon(DTOApplyCouponRequest request)
        {
            var result = await cartBillBLL.ApplyCoupon(request, true);
            return Ok(result);
        }
        
    }
}
