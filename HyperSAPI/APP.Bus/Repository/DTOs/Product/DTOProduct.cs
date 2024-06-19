using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Product
{
    public class DTOProduct
    {
        public int Code { get; set; }

        public string IdProduct { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int CodeProductType { get; set; }

        public string ProductType { get; set; } = string.Empty;

        public int CodeBrand { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public int Price { get; set; }

        public string? Description { get; set; } = string.Empty;

        public int? Stock { get; set; } = 0;

        public int? Sold { get; set; } = 0;

        public string? Color { get; set; } = string.Empty;

        public int? Gender { get; set; } = 0;

        public int? Discount { get; set; } = null;

        public float? PriceAfterDiscount { get; set; } = null;

        public string? DiscountDescription { get; set; } = string.Empty;

        public int Status { get; set; } = 0;

        public List<DTOImage> ListOfImage { get; set; } = new List<DTOImage>();

        public List<DTOProductSize> ListOfSize { get; set; } = new List<DTOProductSize>();


    }
}
