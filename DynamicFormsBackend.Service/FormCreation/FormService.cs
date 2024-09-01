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
        
       public async Task<(bool success, int formid)> AddSourceTemplate(SourceTemplateDto sourceTemplateDetails, int userId)
        {
            var mappedEntity = new SourceTemplate
            {
                FormName = sourceTemplateDetails.FormName,
                Description = sourceTemplateDetails.Description,
                IsPublish = sourceTemplateDetails.IsPublish,
                Version = sourceTemplateDetails.Version,
                UserId = userId,
                CreatedOn = DateTime.Now,
                CreatedBy = userId,
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

                if(createdSection == null)
                {
                    return (false, res.Id);
                }


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


        public async Task<FetchFormDto> GetFormById(int formId, int userId)
        {


            var formEntity = await _formRepository.GetSourceTemplateById(formId, userId);

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

                Sections = formEntity.TemplateSections
                    .Where(section => section.Active == true) // Filter inactive sections
                    .Select(section => new FormSectionDto
                    {
                        Id = section.Id,
                        SectionName = section.SectionName,
                        Description = section.Description,
                        Slno = section.Slno,
                        Active = section.Active,

                        Questions = section.QuestionSectionMappings
                            .Where(qsm => qsm.Active == true) // Filter inactive question-section mappings
                            .Select(qsm => new FormQuestionDto
                            {
                                Id = qsm.Question.Id,
                                Question = qsm.Question.Question,
                                slno = qsm.Question.Slno,
                                AnswerTypeId = qsm.Question.AnswerTypeId,
                                Required = qsm.Question.Required,
                                DataType = qsm.Question.DataType,
                                Constraint = qsm.Question.Constraint,
                                ConstraintValue = qsm.Question.ConstraintValue,
                                WarningMessage = qsm.Question.WarningMessage,
                                Active = qsm.Active,

                                AnswerOptions = qsm.Question.AnswerMasters
                                    .Where(am => am.Active == true) // Filter inactive answer options
                                    .Select(am => new FormAnswerOptionDto
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


        public async Task<bool> RemoveFormById(int formId , int userId)
        {
            return await _formRepository.SoftDeleteFormAsync(formId, userId);
        }


        public async Task<IEnumerable<SourceTemplate>> Getforms(int userId)
        {
            var forms = await _formRepository.GetSourceTemplates(userId);
            return forms;
        }





        public async Task<bool> UpdateSourceTemplate(int formId, SourceTemplateDto sourceTemplateDetails, int userId)
        {
            var existingTemplate = await _formRepository.GetSourceTemplateById(formId, userId);

            if (existingTemplate == null)
            {
                return false;
            }

            // Update the template properties
            existingTemplate.FormName = sourceTemplateDetails.FormName;
            existingTemplate.Description = sourceTemplateDetails.Description;
            existingTemplate.IsPublish = sourceTemplateDetails.IsPublish;
            existingTemplate.Version = sourceTemplateDetails.Version + 1;
            existingTemplate.ModifiedBy = userId;
            existingTemplate.ModifiedOn = DateTime.Now;

            // Update the template in the database
            await _formRepository.UpdateSourceTemplate(existingTemplate);


            // Get existing sections for this form
            var existingSections = await _formRepository.GetSectionsByFormId(formId);
            var existingSectionIds = existingSections.Select(s => s.Id).ToList();

            // Prepare list to track new section IDs
            var newSectionIds = new List<int>();

            foreach (var sectionDto in sourceTemplateDetails.Sections)
            {
                if (sectionDto.Id > 0)
                {
                    // Update existing section
                    var existingSection = await _formRepository.GetSectionById(sectionDto.Id.Value);

                    if (existingSection != null)
                    {
                        existingSection.SectionName = sectionDto.SectionName;
                        existingSection.Description = sectionDto.Description;
                        existingSection.Slno = sectionDto.Slno;
                        existingSection.ModifiedBy = userId;
                        existingSection.ModifiedOn = DateTime.Now;
                        await _formRepository.UpdateSection(existingSection);
                        newSectionIds.Add(existingSection.Id);

                        // Update question mappings for the existing section
                        await UpdateQuestionMappings(existingSection.Id, sectionDto.SelectedQuestions);
                    
                    }
                }
                else
                {
                    // Create new section
                    var newSection = new TemplateSection
                    {
                        FormId = existingTemplate.Id,
                        SectionName = sectionDto.SectionName,
                        Description = sectionDto.Description,
                        Active = true,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = existingTemplate.CreatedBy
                    };
                    await _formRepository.InsertSection(newSection);
                    newSectionIds.Add(newSection.Id);

                    // Update question mappings for the new section
                    await UpdateQuestionMappings(newSection.Id, sectionDto.SelectedQuestions);
                }
            }

            // Soft delete sections that are no longer present in the updated list
            var sectionsToDelete = existingSectionIds.Except(newSectionIds).ToList();
            foreach (var sectionId in sectionsToDelete)
            {
                var sectionToDelete = await _formRepository.GetSectionById(sectionId);
                if (sectionToDelete != null)
                {
                    sectionToDelete.Active = false;
                    await _formRepository.UpdateSection(sectionToDelete);
                }
            }

            // Update question mappings for each section
/*            foreach (var sectionDto in sourceTemplateDetails.Sections)
            {
                await UpdateQuestionMappings(sectionDto.Id ?? 0, sectionDto.SelectedQuestions);
            }*/

            // Save all changes in one transaction
            await _formRepository.SaveChangesAsync();

            return true;
        }



        private async Task UpdateQuestionMappings(int sectionId, List<int> selectedQuestions)
        {
            // Remove existing mappings
            var existingMappings = await _formRepository.GetQuestionMappingsBySectionId(sectionId);

            foreach (var mapping in existingMappings)
            {
                mapping.Active = false; // Soft delete
            }

            // Create new mappings
            foreach (var questionId in selectedQuestions)
            {
                var newMapping = new QuestionSectionMapping
                {
                    QuestionId = questionId,
                    SectionId = sectionId,
                    Active = true,
                };
                await _formRepository.InsertQuestionSectionMappingEntry(newMapping);
            }
        }





    }
}