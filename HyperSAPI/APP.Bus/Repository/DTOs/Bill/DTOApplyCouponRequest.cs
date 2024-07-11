using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOApplyCouponRequest
    {
        public string IdCoupon { get; set; }
        public int TotalBill { get; set; }
        public bool IsGuess {  get; set; }
    }
}
