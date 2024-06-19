using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class DTOCart
    {
        public int? CodeCustomer { get; set; }

        public List<DTOProductInCart> ListProductInCart { get; set; } = new List<DTOProductInCart>();

    }
}
