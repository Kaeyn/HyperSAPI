using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class User
{
    public int Code { get; set; }

    public string? IdUser { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    /// <summary>
    /// 0: Normal;\n1: Blocked;
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 0: Customer;\n1: Admin;\n2: Staff;
    /// </summary>
    public int Permission { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
