﻿using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs.Staff;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private StaffBLL _BLL;

        public StaffController()
        {
            _BLL = new StaffBLL();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> GetStaff(DTOStaff request)
        {
            var products = await _BLL.GetStaff(request);
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> GetListStaff([FromBody] dynamic request)
        {
            var products = await _BLL.GetListStaff(request);
            return Ok(products);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult> GetCurrentStaffInfo()
        {
            var userID = User.FindFirstValue(ClaimTypes.Name);
            var products = await _BLL.GetCurrentStaffInfo(userID);
            return Ok(products);
        }

        /*[HttpPost]
        public ActionResult UpdateStaff([FromBody] dynamic request)
        {
            var products = _BLL.UpdateStaff(request);
            return Ok(products);
        }*/
    }
}
