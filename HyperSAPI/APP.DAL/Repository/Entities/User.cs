using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class User
{
    public string IdUser { get; set; } = null!;

    public int Code { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateOnly? Birth { get; set; }

    /// <summary>
    /// 0: Male;
    /// 1: Female;
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 0: Normal;
    /// 1: Blocked;
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 0: Customer;
    /// 1: Admin;
    /// 2: Staff;
    /// </summary>
    public int Permission { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
