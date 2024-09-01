using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class DynamicFormUser
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<FormQuestion> FormQuestions { get; set; } = new List<FormQuestion>();

    public virtual DynamicFormsRole? Role { get; set; }

    public virtual ICollection<SourceTemplate> SourceTemplates { get; set; } = new List<SourceTemplate>();
}
