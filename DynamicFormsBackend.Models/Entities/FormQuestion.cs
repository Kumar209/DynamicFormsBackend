using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class FormQuestion
{
    public int Id { get; set; }

    public string Question { get; set; } = null!;

    public int? Slno { get; set; }

    public string? Size { get; set; }

    public bool? Required { get; set; }

    public int? AnswerTypeId { get; set; }

    public string? DataType { get; set; }

    public string? Constraint { get; set; }

    public string? ConstraintValue { get; set; }

    public string? WarningMessage { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<AnswerMaster> AnswerMasters { get; set; } = new List<AnswerMaster>();

    public virtual ICollection<QuestionSectionMapping> QuestionSectionMappings { get; set; } = new List<QuestionSectionMapping>();

    public virtual DynamicFormUser? User { get; set; }
}
