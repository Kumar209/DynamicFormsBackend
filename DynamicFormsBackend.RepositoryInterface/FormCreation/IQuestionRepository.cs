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

        public Task<IEnumerable<FormQuestion>> GetQuestions(int userId);

        public Task<FormQuestion> GetQuestionById(int id, int userId);

        public Task<(bool success, int questionId)> InsertQuestion(FormQuestion questionDetail);

        public Task<AnswerOption> InsertAnswerOptions(AnswerOption optionDetails);

        public Task<AnswerMaster> InsertAnswerMaster(AnswerMaster data);

        public Task<bool> DeleteQuestionAsync(int questionId, int userId);




        // Update API
        public Task UpdateQuestion(FormQuestion questionDetail);

        public Task<AnswerOption> GetAnswerOptionById(int id);

        public Task<IEnumerable<AnswerMaster>> GetAnswerMastersByQuestionId(int questionId);

        public Task<IEnumerable<AnswerMaster>> GetAnswerMastersByAnswerOptionId(int optionId);


        public Task UpdateAnswerOption(AnswerOption optionDetails);

        public Task UpdateAnswerMaster(AnswerMaster data);

    }
}
