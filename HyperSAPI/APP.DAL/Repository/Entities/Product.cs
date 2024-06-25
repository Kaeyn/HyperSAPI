using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Product
{
    public int Code { get; set; }

    public string IdProduct { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int ProductType { get; set; }

    public int Brand { get; set; }

    public int Price { get; set; }

    public string? Description { get; set; }

    public string? Color { get; set; }

    public int? Gender { get; set; }

    public int? Discount { get; set; }

    public string? DiscountDescription { get; set; }

    /// <summary>
    /// 0: Normal
    /// 1: New
    /// </summary>
    public int? IsNew { get; set; }

    public int Status { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();

    public virtual Brand BrandNavigation { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ProductType ProductTypeNavigation { get; set; } = null!;
}
