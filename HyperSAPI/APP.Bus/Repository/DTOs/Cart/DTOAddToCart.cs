using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class DTOAddToCart
    {
        public int CodeProduct { get; set; }

        public int CodeCustomer { get; set; }

        public int SelectedSize { get; set; }

        public int Quantity { get; set; }

        public string Type { get; set; }
    }
}
