using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.FormCreation;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Service.FormCreation
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }



        public async Task<IEnumerable<AnswerTypesDto>> GetAnswerTypes()
        {
            var answerTypeEnitity = await _questionRepository.GetAnswerTypes();



            var answerTypesMapped = answerTypeEnitity.Select(a => new AnswerTypesDto
            {
                Id = a.Id,
                TypeName = a.TypeName

            }).ToList();

            return answerTypesMapped;
        }



        public async Task<IEnumerable<AllQuestionDto>> GetAllQuestions()
        {
            var res = await _questionRepository.GetQuestions();


            var mappedDto = res.Select(ans => new AllQuestionDto
            {
                Id = ans.Id,
                Question = ans.Question,
                Slno = ans.Slno,
                Size = ans.Size,
                Required = ans.Required,
                DataType = ans.DataType,
                Constraint = ans.Constraint,
                ConstraintValue = ans.ConstraintValue,
                WarningMessage = ans.WarningMessage,
                AnswerTypeId = ans.AnswerTypeId,
                AnswerOptions = ans.AnswerMasters
                                .Select(am => new AnswerOptionFormDto
                                {
                                    Id = am.AnswerOption.Id,
                                    OptionValue = am.AnswerOption?.OptionValue,
                                    NextQuestionId = am.NextQuestionId
                                })
                                .Where(option => option.OptionValue != null)
                                .ToList(),
            }
            ).ToList();


            return mappedDto;
        }



        public async Task<(bool success, int questionId)> AddQuestion(QuestionDto questionDetail)
        {
            var mappedData = new FormQuestion
            {
                Question = questionDetail.Question,
                Slno = questionDetail.Slno,
                Size = string.IsNullOrWhiteSpace(questionDetail.Size) ? null : questionDetail.Size,
                AnswerTypeId = questionDetail.AnswerTypeId,
                Required = questionDetail.Required,
                DataType = questionDetail.DataType,
                Constraint = questionDetail.Constraint,
                ConstraintValue = questionDetail.ConstraintValue,
                WarningMessage = questionDetail.WarningMessage,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                Active = true,
            };

            var QuestionEnitityInserted = await _questionRepository.InsertQuestion(mappedData);


            // Handle Options if they exist
            if (questionDetail.AnswerOptions != null)
            {
                foreach (var option in questionDetail.AnswerOptions)
                {
                    var answerOption = new AnswerOption
                    {
                        AnswerTypeId = questionDetail.AnswerTypeId,
                        OptionValue = option.OptionValue,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        Active = true,
                    };

                     var answerOptionEntityInserted =  await _questionRepository.InsertAnswerOptions(answerOption);




                    var answerMaster = new AnswerMaster
                    {
                        QuestionId = QuestionEnitityInserted.questionId,
                        AnswerOptionId = answerOptionEntityInserted.Id,
                        NextQuestionId = option.NextQuestionID
                    };

                    var answerMasterEntityInserted = await _questionRepository.InsertAnswerMaster(answerMaster);
                }
            }


            return QuestionEnitityInserted;
        }



        public async Task<QuestionDto> GetQuestion(int questionId)
        {
            var question = await _questionRepository.GetQuestionById(questionId);

            if(question == null)
            {
                return null;
            }

            // Safe access to the AnswerType property
            var answerOption = question.AnswerMasters
                .Select(am => am.AnswerOption)
                .FirstOrDefault(ao => ao != null);

            var answerType = answerOption?.AnswerType?.TypeName ?? string.Empty;

            var mappedQuestion = new QuestionDto
            {
                Id = questionId,
                Question = question.Question,
                Slno = question.Slno ?? 0,
                Size = question.Size,
                Required = question.Required,
                AnswerTypeId = question.AnswerTypeId ?? 0,
                DataType = question.DataType,
                Constraint = question.Constraint,
                ConstraintValue = question.ConstraintValue,
                WarningMessage = question.WarningMessage,
                CreatedBy = question.CreatedBy,



                // Map AnswerOptions to OptionDto
                AnswerOptions = question.AnswerMasters
                                .Select(am => new OptionDto
                                {
                                    Id = am.AnswerOption?.Id, 
                                    OptionValue = am.AnswerOption?.OptionValue,
                                    NextQuestionID = am.NextQuestionId 
                                })
                                .Where(option => option.OptionValue != null)
                                .ToList(),

            };

            return mappedQuestion;


        }



        public async Task<bool> DeleteQuestionById(int questionId)
        {
            return await _questionRepository.DeleteQuestionAsync(questionId);
        }




    }
}
