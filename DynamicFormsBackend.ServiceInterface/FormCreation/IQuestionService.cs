using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.ServiceInterface.FormCreation
{
    public interface IQuestionService
    {
        public Task<IEnumerable<AnswerTypesDto>> GetAnswerTypes();

        public Task<IEnumerable<AllQuestionDto>> GetAllQuestions();

        public Task<QuestionDto> GetQuestion(int questionId);

        public Task<(bool success, int questionId)> AddQuestion(QuestionDto questionDetail);

        public Task<bool> DeleteQuestionById(int questionId);

        public Task<(bool success, string message)> UpdateQuestion(QuestionDto questionDetail);

    }
}
