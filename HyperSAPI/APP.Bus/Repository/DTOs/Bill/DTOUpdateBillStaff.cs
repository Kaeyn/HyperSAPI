using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOUpdateBillStaff : DTOUpdateBill
    {
        public int TotalBill {  get; set; }
    }
}
