using APP.Bus.Repository.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Brands
{
    public class DTOUpdateBrandRequest
    {
        public DTOBrand Brand { get; set; }
        public List<String> Properties { get; set; } = new List<String>();
    }
}
