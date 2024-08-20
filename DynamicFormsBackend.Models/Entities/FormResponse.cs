using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class FormResponse
{
    public int Id { get; set; }

    public int? FormId { get; set; }

    public string? Response { get; set; }

    public string? Email { get; set; }

    public int? AnswerMasterId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public virtual AnswerMaster? AnswerMaster { get; set; }

    public virtual SourceTemplate? Form { get; set; }
}
