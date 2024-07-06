using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOBill
    {
        public int Code { get; set; }
        public string CustomerName { get; set; }
        public string OrdererPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreateAt { get; set; }
        public int PaymentMethod { get; set; }
        public List<DTOBillInfo> ListBillInfo { get; set; } = new List<DTOBillInfo>();
        public int Status { get; set; }
        public string Voucher { get; set; }
        public int TotalDiscount { get; set; }
        public int TotalBill { get; set; }
    }
}
