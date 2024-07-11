using APP.Bus.Repository.BLLs;
using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.Services;
using APP.DAL.Repository.Entities;
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
        private BillBLL _BillBLL;
        private IVnPayService VnPayService;

        public CartController(IVnPayService vnPayService)
        {
            _BLL = new CartBLL();
            _BillBLL = new BillBLL();
            VnPayService = vnPayService;
        }


        [HttpPost]
        public ActionResult GetListCartProduct([FromBody] dynamic options)
        {
            var products = _BLL.GetListCartProduct(options);
            if (products.ObjectReturn?.Data == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult ProceedToPayment([FromBody] dynamic options)
        {
            var dbcheck = _BLL.ProceedToPayment(options, null, false);
            if (dbcheck.ErrorString == "Payment")
            {
                var response = dbcheck.ObjectReturn;
                int orderID = response.Code;
                double total = response.Total;
                int paymentMethod = response.PaymentMethod;
                if (paymentMethod == 2)
                {
                    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                    VnPaymentRequestModel model = new VnPaymentRequestModel
                    {
                        OrderId = orderID,
                        FullName = "TestFullName",
                        Description = "TestDescription",
                        Amount = total,
                        CreatedDate = vietnamTime
                    };
                    string paymentUrl = VnPayService.CreatePaymentUrl(HttpContext, model);
                    dbcheck.ErrorString = "";
                    dbcheck.ObjectReturn = new { RedirectUrl = paymentUrl };
                    _BillBLL.UpdatePaymentString(orderID, paymentUrl);
                }

            }
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
                    /*_BillBLL.SuccessPaymentUpdate()*/
                    int codeBill = ExtractOrderId(paymentResponse.OrderDescription);
                    _BillBLL.SuccessPaymentUpdate(codeBill);
                    return Redirect("http://localhost:4200/HyperS/ecom/home");
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

        public static int ExtractOrderId(string orderDescription)
        {
            // Split the string by ':'
            string[] parts = orderDescription.Split(':');

            // Get the order ID part (assuming it's always the last part after ':')
            string orderIdStr = parts[parts.Length - 1].Trim();

            // Convert orderIdStr to an integer (if needed)
            if (int.TryParse(orderIdStr, out int orderId))
            {
                return orderId;
            }
            else
            {
                // Handle parse failure if needed
                throw new ArgumentException("Invalid order description format");
            }
        }
    }
}
