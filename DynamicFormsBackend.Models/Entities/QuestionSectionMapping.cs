using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class QuestionSectionMapping
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? SectionId { get; set; }

    public bool? Active { get; set; }

    public virtual FormQuestion? Question { get; set; }

    public virtual TemplateSection? Section { get; set; }
}
