﻿using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Cart
{
    public int Code { get; set; }

    public int CodeProduct { get; set; }

    public int CodeCustomer { get; set; }

    public int SelectedSize { get; set; }

    public int Quantity { get; set; }

    public virtual Customer CodeCustomerNavigation { get; set; } = null!;

    public virtual Product CodeProductNavigation { get; set; } = null!;
}
