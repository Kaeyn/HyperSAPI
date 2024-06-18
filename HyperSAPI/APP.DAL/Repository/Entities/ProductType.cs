using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class ProductType
{
    public int Code { get; set; }

    public string? IdProductType { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
