using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{
    public class OptionDto
    {
        public int? Id { get; set; }
        public string OptionValue { get; set; }
        public int? NextQuestionID { get; set; } 
    }


    /*    public class AnswerMasterDto
        {
            public int Id { get; set; }

            public int QuestionId { get; set; }

            public int AnswerOptionId { get; set; }

            public int? NextQuestionId { get; set; }

        }*/


    public class QuestionDto
    {
        public int? Id { get; set; }

        public string Question { get; set; }

        public int? Slno { get; set; }

        public string? Size { get; set; }

        public bool? Required { get; set; }

        public string? DataType { get; set; }

        public string? Constraint { get; set; }

        public string? ConstraintValue { get; set; }

        public string? WarningMessage { get; set; }

        public int? CreatedBy { get; set; }

        public int AnswerTypeId { get; set; }

        public IEnumerable<OptionDto>? AnswerOptions { get; set; }

/*        public IEnumerable<AnswerMasterDto>? AnswerMaster { get; set; }*/

    }
}
