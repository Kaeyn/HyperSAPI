using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Position
{
    public int Code { get; set; }

    public string IdPosition { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
