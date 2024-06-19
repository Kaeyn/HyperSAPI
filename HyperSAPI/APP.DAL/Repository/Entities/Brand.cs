using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Brand
{
    public int Code { get; set; }

    public string? IdBrand { get; set; }

    public string? BrandName { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
