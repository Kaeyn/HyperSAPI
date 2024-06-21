using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.User
{
    public class DTORegister
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
    }
}
