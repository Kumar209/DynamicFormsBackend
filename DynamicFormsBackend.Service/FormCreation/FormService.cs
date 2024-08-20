using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.FormCreation;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Service.FormCreation
{
    public class FormService : IFormService
    {

       private readonly IFormRepository _formRepository;

       public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }
        
       public async Task<(bool success, int formid)> AddSourceTemplate(SourceTemplateDto sourceTemplateDetails)
        {
            var mappedEntity = new SourceTemplate
            {
                FormName = sourceTemplateDetails.FormName,
                Description = sourceTemplateDetails.Description,
                IsPublish = sourceTemplateDetails.IsPublish,
                Version = sourceTemplateDetails.Version,
                UserId = 1,
                CreatedOn = DateTime.Now,
                CreatedBy = 1,
                Active = true,
            };

            var res = await _formRepository.AddSourceTemplate(mappedEntity);



            foreach (var sectionDto in sourceTemplateDetails.Sections)
            {
                var mappedSection = new TemplateSection
                {
                    FormId = res.Id,
                    SectionName = sectionDto.SectionName,
                    Slno = sectionDto.Slno,
                    Description = sectionDto.Description,
                    Active = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = res.CreatedBy
                };

                var createdSection = await _formRepository.InsertSection(mappedSection);


                foreach (var questionId in sectionDto.SelectedQuestions)
                {
                    var CreatedQuestionSectionMapping = new QuestionSectionMapping
                    {
                        QuestionId = questionId,
                        SectionId = createdSection.Id
                    };

                    await _formRepository.InsertQuestionSectionMappingEntry(CreatedQuestionSectionMapping);
                }
            }

            

            return (true, res.Id);
        }


        public async Task<FetchFormDto> GetFormById(int formId)
        {
            var formEntity = await _formRepository.GetSourceTemplateById(formId);

            if (formEntity == null)
            {
                return null;
            }

            var formDto = new FetchFormDto
            {
                Id = formEntity.Id,
                Name = formEntity.FormName,
                Description = formEntity.Description,
                IsPublish = formEntity.IsPublish,
                Version = formEntity.Version,
                Active = formEntity.Active,

                Sections = formEntity.TemplateSections.Select(section => new FormSectionDto
                {
                    Id = section.Id,
                    SectionName = section.SectionName,
                    Description = section.Description,
                    Slno = section.Slno,
                    Active = section.Active,

                    Questions = section.QuestionSectionMappings.Select(qsm => new FormQuestionDto
                    {
                        Id = qsm.Question.Id,
                        Question = qsm.Question.Question,
                       /* AnswerType = qsm.Question.AnswerType.TypeName,*/
                        AnswerTypeId = qsm.Question.AnswerTypeId,
                        DataType = qsm.Question.DataType,
                        Constraint = qsm.Question.Constraint,
                        ConstraintValue = qsm.Question.ConstraintValue,
                        WarningMessage = qsm.Question.WarningMessage,
                        Active = qsm.Active,

                        AnswerOptions = qsm.Question.AnswerMasters.Select(am => new FormAnswerOptionDto
                        {
                            Id = am.AnswerOption.Id,
                            OptionValue = am.AnswerOption.OptionValue,
                            NextQuestionId = am.NextQuestionId,
                            Active = am.Active
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return formDto;
        }


        public async Task<bool> RemoveFormById(int formId)
        {
            return await _formRepository.SoftDeleteFormAsync(formId);
        }


        public async Task<IEnumerable<SourceTemplate>> Getforms()
        {
            var forms = await _formRepository.GetSourceTemplates();
            return forms;
        }
    }
}