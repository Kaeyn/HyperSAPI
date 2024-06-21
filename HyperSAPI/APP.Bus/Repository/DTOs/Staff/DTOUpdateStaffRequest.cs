using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Staff
{
    public class DTOUpdateStaffRequest
    {
        public DTOStaff Staff { get; set; }
        public List<String> Properties { get; set; } = new List<String>();
    }
}
