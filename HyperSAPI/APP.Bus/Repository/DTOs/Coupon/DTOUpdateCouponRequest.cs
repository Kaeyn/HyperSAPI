using APP.Bus.Repository.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Coupon
{
    public class DTOUpdateCouponRequest
    {
        public DTOCoupon Coupon { get; set; }
        public List<String> Properties { get; set; } = new List<String>();
    }
}
