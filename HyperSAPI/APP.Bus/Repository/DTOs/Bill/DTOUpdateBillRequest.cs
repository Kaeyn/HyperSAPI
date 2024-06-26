using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOUpdateBillRequest
    {
        public int CodeBill {  get; set; }
        public int Status {  get; set; }

        public string Note { get; set; }
    }
}
