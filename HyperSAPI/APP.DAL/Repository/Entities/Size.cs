using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Size
{
    public int Code { get; set; }

    public string? IdSize { get; set; }

    public int Size1 { get; set; }

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
