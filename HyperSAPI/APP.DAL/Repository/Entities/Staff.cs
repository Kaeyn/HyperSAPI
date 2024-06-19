using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Staff
{
    public int Code { get; set; }

    public int CodeUser { get; set; }

    public string? Idstaff { get; set; }

    public string? Name { get; set; }

    public DateOnly? Birthday { get; set; }

    public int? Gender { get; set; }

    public string? Identication { get; set; }

    public string? ImageUrl { get; set; }

    public virtual User CodeUserNavigation { get; set; } = null!;
}
