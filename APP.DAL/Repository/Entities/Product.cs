using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Product
{
    public int Code { get; set; }

    public string IdProduct { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public double Price { get; set; }

    public string? Description { get; set; }

    public int Stock { get; set; }

    public int Brand { get; set; }

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
