using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Banner
{
    public int Code { get; set; }

    public string? Title { get; set; }

    public int? BannerType { get; set; }

    public string? BannerUrl { get; set; }

    public int? Position { get; set; }

    public string? Page { get; set; }

    public int? Status { get; set; }
}
