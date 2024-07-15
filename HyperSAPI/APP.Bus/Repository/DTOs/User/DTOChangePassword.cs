using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.User
{
    public class DTOChangePassword
    {
        public string Email { get; set; }
        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string? Token { get; set; }
    }
}
