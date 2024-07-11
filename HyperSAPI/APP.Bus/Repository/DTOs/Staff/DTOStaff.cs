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
        public string? IdStaff { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Identication { get; set; }
        public List<dynamic> ListShift { get; set; }
        public float TotalSalary { get; set; }
        public int CodeAccount { get; set; }
        public int Status { get; set; }
        public string StatusAccountStr { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }
}
