using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOUpdateBill
    {
        public int CodeBill { get; set; }
        public int Status { get; set; }
        public List<DTOBillInfo> ListOfBillInfo { get; set; } = new List<DTOBillInfo>();
        public string Note { get; set; }
    }
}
