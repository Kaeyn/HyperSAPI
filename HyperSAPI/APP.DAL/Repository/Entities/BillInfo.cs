using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class BillInfo
{
    public int Code { get; set; }

    public int CodeBill { get; set; }

    public int CodeProduct { get; set; }

    public int SelectedSize { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int? Discount { get; set; }

    public int? TotalPriceBeforeDiscount { get; set; }

    public int TotalPrice { get; set; }

    public int Status { get; set; }

    public virtual Bill CodeBillNavigation { get; set; } = null!;

    public virtual Product CodeProductNavigation { get; set; } = null!;
}
