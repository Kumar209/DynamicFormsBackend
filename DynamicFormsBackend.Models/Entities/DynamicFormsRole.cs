using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class DynamicFormsRole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DynamicFormUser> DynamicFormUsers { get; set; } = new List<DynamicFormUser>();
}
