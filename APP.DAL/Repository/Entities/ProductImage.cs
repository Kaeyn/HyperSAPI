using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class ProductImage
{
    public uint Code { get; set; }

    public string IdImage { get; set; } = null!;

    public string Img { get; set; } = null!;

    public int ProductCode { get; set; }

    public virtual Product ProductCodeNavigation { get; set; } = null!;
}
