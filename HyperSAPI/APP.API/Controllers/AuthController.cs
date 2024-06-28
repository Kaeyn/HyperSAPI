using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs.Product;
using APP.Bus.Repository.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(SignInManager<IdentityUser> sm, UserManager<IdentityUser> um, RoleManager<IdentityRole> rm) : ControllerBase
    {
        private UserBLL _BLL = new UserBLL(sm, um, rm);

        [HttpPost]
        public async Task<ActionResult> ResgisterUser(dynamic request)
        {
            var products = await _BLL.RegisterUserAsync(request);
            /*if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }*/
            return Ok(products);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> LogOut()
        {
            var result = await _BLL.LogOut();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Login(dynamic request)
        {
            var products = await _BLL.LoginUserAsync(request);
            /*if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }*/
            return Ok(products);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> TestAdmin()
        {    
            return Ok("ADMIN");
        }

    }
}
