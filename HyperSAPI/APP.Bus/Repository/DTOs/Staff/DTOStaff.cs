using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.Staff
{
    public class DTOStaff
    {
        public int Code { get; set; }
        public string? IDStaff { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
        public int? Gender { get; set; }
        public DateOnly? Birth { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? Identication { get; set; }

        public int Position { get; set; }
        public string PositionStr { get; set; }
        public List<dynamic> ListShift { get; set; }
        public float TotalSalary { get; set; }
        public int CodeAccount { get; set; }
        public int StatusAccount { get; set; }
        public string StatusAccountStr { get; set; } = string.Empty;
        public int Permission { get; set; }
        public string PermissionStr { get; set; } = string.Empty;
    }
}
