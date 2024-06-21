using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class AspNetUserClaim
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public virtual User User { get; set; } = null!;
}
