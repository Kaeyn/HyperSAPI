using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Bill
{
    public int Code { get; set; }

    public string CustomerName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    /// <summary>
    /// 0: COD
    /// 1: QR
    /// 
    /// </summary>
    public int PaymentMethod { get; set; }

    public int TotalBill { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
