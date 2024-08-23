﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Models.Dto
{

    public class FormResponseDto
    {
        public int? Id { get; set; }
        public int FormId { get; set; }
        public string Email { get; set; }
        public int? AnswerMasterId { get; set; }
        public IList<ResponseAnswerDto> Responses { get; set; }
    }


    public class ResponseAnswerDto
    {
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    }


    /*public class FormResponseDto
    {
        public int? Id { get; set; }

        public int? FormId { get; set; }

        public string? Response { get; set; }

        public string? Email { get; set; }

        public int? AnswerMasterId { get; set; }

    }*/
}
