using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class ShippingAddress
{
    public int Code { get; set; }

    public int CustomerCode { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ReceiverName { get; set; }

    /// <summary>
    /// 0: False\n1: True
    /// </summary>
    public sbyte? IsDefaultAddress { get; set; }

    public virtual Customer CustomerCodeNavigation { get; set; } = null!;
}
