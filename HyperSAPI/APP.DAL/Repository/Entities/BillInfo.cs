using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class BillInfo
{
    public int Code { get; set; }

    public int CodeBill { get; set; }

    public int CodeProduct { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int? Discount { get; set; }

    public string TotalPrice { get; set; } = null!;

    public virtual Bill CodeBillNavigation { get; set; } = null!;

    public virtual Product CodeProductNavigation { get; set; } = null!;
}
