using System;
using System.Collections.Generic;

namespace DynamicFormsBackend.Models.Entities;

public partial class AnswerMaster
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int AnswerOptionId { get; set; }

    public int? NextQuestionId { get; set; }

    public bool? Active { get; set; }

    public virtual AnswerOption AnswerOption { get; set; } = null!;

    public virtual ICollection<FormResponse> FormResponses { get; set; } = new List<FormResponse>();

    public virtual FormQuestion Question { get; set; } = null!;
}
