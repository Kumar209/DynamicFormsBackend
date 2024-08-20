using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.RepositoryInterface.FormCreation
{
    public interface IQuestionRepository
    {
        public Task<IEnumerable<AnswerType>> GetAnswerTypes();

        public Task<IEnumerable<FormQuestion>> GetQuestions();

        public Task<FormQuestion> GetQuestionById(int id);

        public Task<(bool success, int questionId)> InsertQuestion(FormQuestion questionDetail);

        public Task<AnswerOption> InsertAnswerOptions(AnswerOption optionDetails);

        public Task<AnswerMaster> InsertAnswerMaster(AnswerMaster data);

        public Task<bool> DeleteQuestionAsync(int questionId);

        public Task UpdateQuestion(FormQuestion questionDetail);


    }
}
