using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class AnswerOption
{
    public int Id { get; set; }

    public int? AnswerTypeId { get; set; }

    public string? OptionValue { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<AnswerMaster> AnswerMasters { get; set; } = new List<AnswerMaster>();

    public virtual AnswerType? AnswerType { get; set; }
}
