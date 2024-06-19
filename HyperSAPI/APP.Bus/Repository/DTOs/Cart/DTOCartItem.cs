using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class DTOCartItem
    {
        public int? Code { get; set; } = null;

        public int CodeProduct { get; set; }

        public int CodeCustomer { get; set; }

        public int? Quantity { get; set; }
    }
}
