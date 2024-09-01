using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.FormCreation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DynamicFormsBackend.Repository.FormCreation
{
    public class FormRepository : IFormRepository
    {
        private readonly ApplicationDbContext _context;

        public FormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SourceTemplate> AddSourceTemplate(SourceTemplate template)
        {
            _context.SourceTemplates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }


        public async Task<QuestionSectionMapping> InsertQuestionSectionMappingEntry(QuestionSectionMapping data)
        {
                _context.QuestionSectionMappings.Add(data);
                await _context.SaveChangesAsync();
                return data;
        }

        public async Task<IEnumerable<SourceTemplate>> GetSourceTemplates(int userId)
        {
            var forms = await _context.SourceTemplates
                              .Where(Fq => Fq.Active == true && Fq.UserId == userId)
                              .ToListAsync();

            return forms;

        }


        public async Task<SourceTemplate> GetSourceTemplateById(int formId, int userId)
        {


            var formEntity = await _context.SourceTemplates
                                    .Include(f => f.TemplateSections
                                        .Where(s => s.Active == true)) // Filter active sections
                                    .ThenInclude(s => s.QuestionSectionMappings
                                        .Where(qsm => qsm.Active == true)) // Filter active question-section mappings
                                    .ThenInclude(qsm => qsm.Question)
                                        .ThenInclude(q => q.AnswerMasters
                                            .Where(am => am.Active == true)) // Filter active answer masters
                                    .ThenInclude(am => am.AnswerOption)
                                    .FirstOrDefaultAsync(f => f.Id == formId && f.Active == true && f.UserId == userId);




            return formEntity;
        }




        public async Task<bool> SoftDeleteFormAsync(int formId, int userId)
        {
            var form = await _context.SourceTemplates.FirstOrDefaultAsync(f => f.Id == formId && f.Active == true && f.UserId == userId);

            if (form == null) return false;

            // Soft delete the form
            form.Active = false;
            form.DeletedOn = DateTime.Now;
            form.DeletedBy = userId;

            // Soft delete related sections
            var sections = await _context.TemplateSections
                .Where(s => s.FormId == formId && s.Active == true)
                .ToListAsync();

            foreach (var section in sections)
            {
                section.Active = false;
                section.DeletedOn = DateTime.Now;
                section.DeletedBy = userId;

                // Soft delete related question-section mappings
                var mappings = await _context.QuestionSectionMappings
                    .Where(qsm => qsm.SectionId == section.Id)
                    .ToListAsync();

                foreach (var mapping in mappings)
                {
                    mapping.Active = false;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }




        public async Task<bool> UpdateSourceTemplate(SourceTemplate template)
        {
            _context.SourceTemplates.Update(template);
            await _context.SaveChangesAsync();
            return true;
        }









        //Section API
        public async Task<TemplateSection> InsertSection(TemplateSection section)
        {
            _context.TemplateSections.Add(section);
            await _context.SaveChangesAsync();

            return section;
        }


        public async Task<IEnumerable<TemplateSection>> GetAllSections()
        {
            var sections = await _context.TemplateSections
                                         .Where(s => s.Active == true)
                                         .ToListAsync();

            return sections;
        }


        public async Task<TemplateSection> GetSectionById(int sectionId)
        {
           var section = await _context.TemplateSections
                                .Include(ts => ts.QuestionSectionMappings)
                                    .ThenInclude(qsm => qsm.Question)
                                        .ThenInclude(q => q.AnswerMasters)
                                            .ThenInclude(am => am.AnswerOption)
                                .FirstOrDefaultAsync(ts => ts.Id == sectionId && ts.Active == true);

            return section; 

        }

        public async Task<bool> UpdateSection(TemplateSection section)
        {
            _context.TemplateSections.Update(section);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<QuestionSectionMapping>> GetQuestionMappingsBySectionId(int sectionId)
        {
            return await _context.QuestionSectionMappings
                .Where(qsm => qsm.SectionId == sectionId && qsm.Active == true)
                .ToListAsync();
        }






        // Add a method to save changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<List<TemplateSection>> GetSectionsByFormId(int formId)
        {
            var sections = await _context.TemplateSections
                .Where(s => s.FormId == formId && s.Active == true)
                .Include(s => s.QuestionSectionMappings)
                    .ThenInclude(qsm => qsm.Question)
                        .ThenInclude(q => q.AnswerMasters)
                            .ThenInclude(am => am.AnswerOption)
                .ToListAsync();

            return sections;
        }



    }
}
