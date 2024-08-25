using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{
    public class FetchFormDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsPublish { get; set; }
        public int? Version { get; set; }

        public bool? Active { get; set; }
        public List<FormSectionDto> Sections { get; set; } = new List<FormSectionDto>();
    }



    public class FormSectionDto
    {
        public int Id { get; set; }
        public string SectionName { get; set; }

        public string Description { get; set; }

        public int? Slno { get; set; }

        public bool? Active { get; set; }
        public List<FormQuestionDto> Questions { get; set; } = new List<FormQuestionDto>();
    }



    public class FormQuestionDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int? slno { get; set; }

        public int? AnswerTypeId { get; set; }

        public bool? Required { get; set; }

        public string AnswerType { get; set; }  
        public string? DataType { get; set; }

        public string? Constraint { get; set; }

        public string? ConstraintValue { get; set; }

        public string? WarningMessage { get; set; }

        public bool? Active { get; set; }
        public List<FormAnswerOptionDto> AnswerOptions { get; set; } = new List<FormAnswerOptionDto>();
    }



    public class FormAnswerOptionDto
    {
        public int Id { get; set; }
        public string OptionValue { get; set; }

        public int? NextQuestionId { get; set; }
        public bool? Active { get; set; }

    }

}
