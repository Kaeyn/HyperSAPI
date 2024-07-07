using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Bill
{
    public int Code { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? OrdererPhoneNumber { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    /// <summary>
    /// 0: COD
    /// 1: QR
    /// 
    /// </summary>
    public int PaymentMethod { get; set; }

    public string? Note { get; set; }

    public string? CouponApplied { get; set; }

    public int? CouponDiscount { get; set; }

    public int? TotalBeforeDiscount { get; set; }

    public int TotalBill { get; set; }

    /// <summary>
    /// 0: Chờ xác nhận
    /// 1: Đã xác nhận
    /// 2: Đang đóng gói
    /// 3: Đang vận chuyển
    /// 4: Giao hàng thành công
    /// 5: Giao hàng thất bại
    /// </summary>
    public int Status { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
