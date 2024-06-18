using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class ProductImage
{
    public int Code { get; set; }

    public string IdImage { get; set; } = null!;

    public string Img { get; set; } = null!;

    public int ProductCode { get; set; }

    /// <summary>
    /// 0: FALSE
    /// 1: TRUE
    /// </summary>
    public sbyte IsThumbnail { get; set; }

    public virtual Product ProductCodeNavigation { get; set; } = null!;
}
