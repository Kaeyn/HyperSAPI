using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.Services;
using Mailjet.Client.Resources.SMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace APP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    { 
        private CartBLL _BLL;
        private IVnPayService VnPayService;

        public CartController(IVnPayService vnPayService)
        {
            _BLL = new CartBLL();
            VnPayService = vnPayService;
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
            var dbcheck = _BLL.ProceedToPayment(options, null, false);
            /*if (dbcheck.ErrorString != "Error")
            {
                var response = dbcheck.ObjectReturn;

                double total = response.Total;
                int paymentMethod = response.PaymentMethod;
                if(paymentMethod == 2)
                {
                    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                    VnPaymentRequestModel model = new VnPaymentRequestModel
                    {
                        OrderId = 123,
                        FullName = "TestFullName",
                        Description = "TestDescription",
                        Amount = total,
                        CreatedDate = vietnamTime
                    };
                    string paymentUrl = VnPayService.CreatePaymentUrl(HttpContext, model);
                    return Redirect(paymentUrl);
                }
                
            }*/
            return Ok(dbcheck);
        }

        [HttpGet]
        public ActionResult VnPayReturn([FromQuery] Dictionary<string, string> vnp_Params)
        {
            try
            {
                var paymentResponse = VnPayService.PaymentExecute(Request.Query);
                if (paymentResponse.Success)
                {
                    return Ok(paymentResponse);
                }
                else
                {
                    return BadRequest(paymentResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult GetCountInCart([FromBody] dynamic options)
        {
            var products = _BLL.GetCountInCart(options);
            return Ok(products);
        }

        /*private static string HmacSHA512(string key, string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }*/
    }
}
