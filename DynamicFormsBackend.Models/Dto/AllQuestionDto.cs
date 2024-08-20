using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{
    public class AllQuestionDto
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public int? Slno { get; set; }

        public string? Size { get; set; }

        public bool? Required { get; set; }

        public string? DataType { get; set; }

        public string? Constraint { get; set; }

        public string? ConstraintValue { get; set; }

        public string? WarningMessage { get; set; }

        public int? AnswerTypeId { get; set; }

        public List<AnswerOptionFormDto> AnswerOptions { get; set; } = new List<AnswerOptionFormDto>();
    }

    public class AnswerOptionFormDto
    {
        public int Id { get; set; }
        public string OptionValue { get; set; }

        public int? NextQuestionId { get; set; }
        public bool? Active { get; set; }

    }
}
