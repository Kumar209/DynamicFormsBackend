using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class TemplateSection
{
    public int Id { get; set; }

    public int? FormId { get; set; }

    public string SectionName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Active { get; set; }

    public int? Slno { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public virtual SourceTemplate? Form { get; set; }

    public virtual ICollection<QuestionSectionMapping> QuestionSectionMappings { get; set; } = new List<QuestionSectionMapping>();
}
