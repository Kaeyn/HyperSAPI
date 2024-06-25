using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Product
{
    public class DTOUpdateProductRequest
    {
        public DTOProduct Product { get; set; }
        public List<String> Properties { get; set; } = new List<String>();
    }
}
