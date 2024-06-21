using System;
using System.Collections.Generic;

namespace APP.DAL.Repository.Entities;

public partial class Permission
{
    public int Code { get; set; }

    public string IdPermission { get; set; } = null!;

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
