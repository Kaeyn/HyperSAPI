using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class DTOProccedToPayment
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public int PaymentMethod { get; set; }
        public List<dynamic> ListProduct {  get; set; } = new List<dynamic>();
        public int TotalBill { get; set; }
        public bool IsGuess { get; set; } = true;
    }
}
