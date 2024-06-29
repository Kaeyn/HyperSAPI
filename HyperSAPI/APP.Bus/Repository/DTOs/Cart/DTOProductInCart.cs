using APP.Bus.Repository.DTOs.Product;
using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Cart
{
    public class DTOProductInCart
    {
        public DTOProduct Product { get; set; }

        public int Quantity { get; set; }

        public DTOProductSize SizeSelected { get; set; }

        public float? TotalPriceOfProduct { get; set; }
    }
}
