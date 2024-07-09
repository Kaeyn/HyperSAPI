using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APP.Bus.Repository.DTOs.Bill
{
    public class DTOBillInfo
    {
        public int Code { get; set; }
        public string IDProduct { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public int Size { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public int Status { get; set; }
    }
}
