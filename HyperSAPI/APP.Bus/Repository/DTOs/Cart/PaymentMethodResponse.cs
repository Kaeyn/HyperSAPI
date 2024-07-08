using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class PaymentMethodResponse
    {
        public decimal Total { get; set; }
        public int PaymentMethod { get; set; }
    }
}
