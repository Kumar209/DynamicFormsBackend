using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class AnswerType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
}
