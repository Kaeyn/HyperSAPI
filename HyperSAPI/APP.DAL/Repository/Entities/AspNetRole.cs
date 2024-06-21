using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class AspNetRole
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
