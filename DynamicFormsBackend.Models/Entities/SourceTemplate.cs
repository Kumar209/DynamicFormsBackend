using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class SourceTemplate
{
    public int Id { get; set; }

    public string FormName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsPublish { get; set; }

    public int? Version { get; set; }

    public int? UserId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<FormResponse> FormResponses { get; set; } = new List<FormResponse>();

    public virtual ICollection<TemplateSection> TemplateSections { get; set; } = new List<TemplateSection>();

    public virtual DynamicFormUser? User { get; set; }
}
