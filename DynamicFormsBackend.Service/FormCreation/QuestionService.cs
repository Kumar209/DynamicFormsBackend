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

                    var answerOptionEntityInserted = await _questionRepository.InsertAnswerOptions(answerOption);




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

            if (question == null)
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



                // Map AnswerOptions to OptionDto, filter out inactive options
                AnswerOptions = question.AnswerMasters
                                  .Where(am => am.AnswerOption.Active == true)
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






        public async Task<(bool success, string message)> UpdateQuestion(QuestionDto questionDetail)
        {
            // Get the existing question
            var existingQuestion = await _questionRepository.GetQuestionById(questionDetail.Id ?? 0);

            if (existingQuestion == null)
            {
                return (false, "Question not found");
            }

            // Update the question
            existingQuestion.Question = questionDetail.Question;
            existingQuestion.Slno = questionDetail.Slno;
            existingQuestion.Size = string.IsNullOrWhiteSpace(questionDetail.Size) ? null : questionDetail.Size;
            existingQuestion.AnswerTypeId = questionDetail.AnswerTypeId;
            existingQuestion.Required = questionDetail.Required;
            existingQuestion.DataType = questionDetail.DataType;
            existingQuestion.Constraint = questionDetail.Constraint;
            existingQuestion.ConstraintValue = questionDetail.ConstraintValue;
            existingQuestion.WarningMessage = questionDetail.WarningMessage;
            existingQuestion.ModifiedBy = 1;
            existingQuestion.ModifiedOn = DateTime.Now;
            existingQuestion.Active = true;

            await _questionRepository.UpdateQuestion(existingQuestion);


            // Handle options
            if (questionDetail.AnswerOptions != null)
            {
                var existingAnswerMasters = await _questionRepository.GetAnswerMastersByQuestionId(questionDetail.Id ?? 0);
                var existingAnswerOptions = existingAnswerMasters.Select(am => am.AnswerOptionId).ToList();

                // Convert DTOs to entities
                var answerOptions = questionDetail.AnswerOptions.Select(option => new AnswerOption
                {
                    Id = option.Id ?? 0,
                    AnswerTypeId = questionDetail.AnswerTypeId,
                    OptionValue = option.OptionValue,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now,
                    Active = true,
                }).ToList();

                // Update existing options
                foreach (var option in answerOptions)
                {
                    var existingOption = await _questionRepository.GetAnswerOptionById(option.Id);
                    if (existingOption != null)
                    {
                        existingOption.OptionValue = option.OptionValue;
                        existingOption.ModifiedBy = 1;
                        existingOption.ModifiedOn = DateTime.Now;
                        await _questionRepository.UpdateAnswerOption(existingOption);


                        //Changed Part
                        var existingAnswerMaster = existingAnswerMasters.FirstOrDefault(am => am.AnswerOptionId == option.Id);
                        if (existingAnswerMaster != null)
                        {
                            existingAnswerMaster.NextQuestionId = questionDetail.AnswerOptions
                                .FirstOrDefault(o => o.Id == option.Id)?.NextQuestionID;

                            await _questionRepository.UpdateAnswerMaster(existingAnswerMaster);
                        }
                    }


                    else
                    {
                        // Add new options
                        var newAnswerOptionEntity = await _questionRepository.InsertAnswerOptions(option);

                        // Ensure that the next question ID is valid
                        var answerOptionDetail = questionDetail.AnswerOptions
                            .FirstOrDefault(o => o.Id == option.Id);

                        var newAnswerMaster = new AnswerMaster
                        {
                            QuestionId = questionDetail.Id ?? 0,
                            AnswerOptionId = newAnswerOptionEntity.Id,
                            NextQuestionId = answerOptionDetail?.NextQuestionID // Handle possible null values gracefully
                        };

                        await _questionRepository.InsertAnswerMaster(newAnswerMaster);
                    }
                }

                // Remove deleted options
                var deletedOptions = existingAnswerOptions.Except(answerOptions.Select(o => o.Id)).ToList();
                foreach (var deletedOptionId in deletedOptions)
                {
                    var deletedOption = await _questionRepository.GetAnswerOptionById(deletedOptionId);
                    if (deletedOption != null)
                    {
                        deletedOption.Active = false;
                        deletedOption.DeletedBy = 1;
                        deletedOption.DeletedOn = DateTime.Now;
                        await _questionRepository.UpdateAnswerOption(deletedOption);

                        // Update related answer masters
                        var relatedAnswerMasters = await _questionRepository.GetAnswerMastersByAnswerOptionId(deletedOptionId);
                        foreach (var answerMaster in relatedAnswerMasters)
                        {
                            answerMaster.Active = false;
                            await _questionRepository.UpdateAnswerMaster(answerMaster);
                        }
                    }
                }
            }

            return (true, "Question updated successfully");
        }
    }
}
