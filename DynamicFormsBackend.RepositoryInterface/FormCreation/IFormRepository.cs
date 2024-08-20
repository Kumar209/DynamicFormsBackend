using DynamicFormsBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.RepositoryInterface.FormCreation
{
    public interface IFormRepository
    {
        public Task<SourceTemplate> AddSourceTemplate(SourceTemplate template);

        public Task<QuestionSectionMapping> InsertQuestionSectionMappingEntry(QuestionSectionMapping data);

        public Task<IEnumerable<SourceTemplate>> GetSourceTemplates();

        public Task<SourceTemplate> GetSourceTemplateById(int formId);


        public Task<bool> SoftDeleteFormAsync(int formId);

        public Task<bool> UpdateSourceTemplate(SourceTemplate template);







        //Section API
        public Task<TemplateSection> InsertSection(TemplateSection section);

        public Task<IEnumerable<TemplateSection>> GetAllSections();

        public Task<TemplateSection> GetSectionById(int sectionId);

        public Task<bool> UpdateSection(TemplateSection section);



        public Task<IEnumerable<QuestionSectionMapping>> GetQuestionMappingsBySectionId(int sectionId);

        public Task SaveChangesAsync();
    }
}
