using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APP.Bus.Repository.DTOs.Customer
{
    public class DTOCustomer
    {
        public int Code { get; set; }
        public string? IDCustomer { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
        public int? Gender { get; set; }
        public DateOnly? Birth { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int CodeAccount { get; set; }
        public int StatusAccount { get; set; }
        public string StatusAccountStr { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }
}
