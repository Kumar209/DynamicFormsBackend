using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.FormCreation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Repository.FormCreation
{

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnswerType>> GetAnswerTypes()
        {

            return await _context.AnswerTypes
                  .Where(at => at.Active == true)
                  .ToListAsync();
        }


        public async Task<FormQuestion> GetQuestionById(int id)
        {
            /* var question = await _context.Set<FormQuestion>()
                                 .Include(q => q.AnswerMasters)
                                 .ThenInclude(am => am.AnswerOption)
                                 .ThenInclude(ao => ao.AnswerType)
                                 .FirstOrDefaultAsync(q => q.Id == id && q.Active == true);*/

            var question = await _context.Set<FormQuestion>()
                                 .Include(q => q.AnswerMasters)
                                 .ThenInclude(am => am.AnswerOption)
                                 .ThenInclude(ao => ao.AnswerType)
                                 .Where(q => q.Id == id && q.Active == true)
                                 .FirstOrDefaultAsync();

            if (question != null)
            {
                question.AnswerMasters = question.AnswerMasters.Where(am => am.AnswerOption.Active == true).ToList();
            }

            return question;
        }


        public async Task<IEnumerable<FormQuestion>> GetQuestions()
        {

            var questions = await _context.FormQuestions
                             .Include(q => q.AnswerMasters)
                             .ThenInclude(am => am.AnswerOption)
                             .ThenInclude(ao => ao.AnswerType)
                             .Where(fq => fq.Active == true)
                             .ToListAsync();

            return questions;
        }


        public async Task<(bool success, int questionId)> InsertQuestion(FormQuestion questionDetail)
        {
            _context.FormQuestions.Add(questionDetail);
            await _context.SaveChangesAsync();

            return (true, questionDetail.Id);
        }


        public async Task<AnswerOption> InsertAnswerOptions(AnswerOption optionDetails)
        {
            _context.AnswerOptions.Add(optionDetails);
            await _context.SaveChangesAsync();

            return optionDetails;
        }

        public async Task<AnswerMaster> InsertAnswerMaster(AnswerMaster data)
        {
            _context.AnswerMasters.Add(data);
            await _context.SaveChangesAsync();

            return data;
        }




        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            // Find the question in the database
            var question = await _context.FormQuestions
                                .FirstOrDefaultAsync(fq => fq.Active == true && fq.Id == questionId);

            if (question == null)
            {
                return false; 
            }

            // Soft delete the question
            question.Active = false;
            question.DeletedOn = DateTime.Now;
            question.DeletedBy = 1;

            // Find related answer masters for this question
            var answerMasters = await _context.AnswerMasters
                .Where(am => am.QuestionId == questionId)
                .ToListAsync();

            // Soft delete the answer masters
            foreach (var answerMaster in answerMasters)
            {
                answerMaster.Active = false;
                
            }

            // Find unique answer option IDs from related answer masters
            var answerOptionIds = answerMasters.Select(am => am.AnswerOptionId).Distinct();

            // Soft delete the related answer options
            var answerOptions = await _context.AnswerOptions
                .Where(ao => answerOptionIds.Contains(ao.Id)) // Only delete options related to this question
                .ToListAsync();

            foreach (var option in answerOptions)
            {
                option.Active = false;
                option.DeletedOn = DateTime.Now;
                option.DeletedBy = 1;
            }


            await _context.SaveChangesAsync();

            return true; 
        }








        //Update Question
        public async Task UpdateQuestion(FormQuestion questionDetail)
        {
            _context.FormQuestions.Update(questionDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<AnswerOption> GetAnswerOptionById(int id)
        {
            return await _context.AnswerOptions
                .FirstOrDefaultAsync(ao => ao.Id == id);
        }

        public async Task<IEnumerable<AnswerMaster>> GetAnswerMastersByQuestionId(int questionId)
        {
            return await _context.AnswerMasters
                .Include(am => am.AnswerOption)
                .Where(am => am.QuestionId == questionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AnswerMaster>> GetAnswerMastersByAnswerOptionId(int optionId)
        {
            return await _context.AnswerMasters
                .Where(am => am.AnswerOptionId == optionId)
                .ToListAsync();
        }

        public async Task UpdateAnswerOption(AnswerOption optionDetails)
        {
            _context.AnswerOptions.Attach(optionDetails);
            _context.Entry(optionDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerMaster(AnswerMaster data)
        {
            _context.AnswerMasters.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
