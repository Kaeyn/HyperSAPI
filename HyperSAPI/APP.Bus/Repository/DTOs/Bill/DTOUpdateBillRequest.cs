using APP.Bus.Repository.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOUpdateBillRequest
    {
        public DTOUpdateBill DTOUpdateBill { get; set; }
        public DTOProceedToPayment DTOProceedToPayment { get; set; }
    }

    public class DTOUpdateBillStaffRequest
    {
        public DTOUpdateBillStaff DTOUpdateBill { get; set; }
        public DTOProceedToPayment DTOProceedToPayment { get; set; }
    }
}
