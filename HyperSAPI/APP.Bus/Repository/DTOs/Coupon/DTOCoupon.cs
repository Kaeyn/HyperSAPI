using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Coupon
{
    public class DTOCoupon
    {
        public int Code { get; set; }

        public string? IdCoupon { get; set; }

        public string? Description { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public int? Quantity { get; set; }

        public int? RemainingQuantity { get; set; }

        public int? MinBillPrice { get; set; }

        public int? MaxBillDiscount { get; set; }

        public int? Status { get; set; }

        public int? Stage { get; set; }

        public int? CouponType { get; set; }

        public int? DirectDiscount { get; set; }

        public int? PercentDiscount { get; set; }

        public int? ApplyTo { get; set; }
    }
}
