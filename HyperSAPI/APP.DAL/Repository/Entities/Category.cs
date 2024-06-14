using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Category
{
    public int Code { get; set; }

    public string? IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public string? ParentId { get; set; }
}
