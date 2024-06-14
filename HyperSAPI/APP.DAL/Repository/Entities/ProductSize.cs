using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class ProductSize
{
    public int Code { get; set; }

    public int CodeProduct { get; set; }

    public int? CodeSize { get; set; }

    public virtual Product CodeProductNavigation { get; set; } = null!;

    public virtual Size? CodeSizeNavigation { get; set; }
}
