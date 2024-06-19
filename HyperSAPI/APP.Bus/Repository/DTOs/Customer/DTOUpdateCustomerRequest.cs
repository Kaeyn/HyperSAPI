using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Customer
{
    public class DTOUpdateCustomerRequest
    {
        public int CodeAccount {  get; set; }
        public int CodeStatus { get; set; }
    }
}
