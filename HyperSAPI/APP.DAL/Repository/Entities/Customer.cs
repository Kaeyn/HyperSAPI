using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Customer
{
    public int Code { get; set; }

    public int CodeUser { get; set; }

    public string? Name { get; set; }

    public DateOnly? Birth { get; set; }

    public int? Gender { get; set; }

    public string? Avartar { get; set; }

    public virtual User CodeUserNavigation { get; set; } = null!;

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
}
