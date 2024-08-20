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

        public async Task<IEnumerable<SourceTemplate>> GetSourceTemplates()
        {
            var forms = await _context.SourceTemplates
                              .Where(Fq => Fq.Active == true)
                              .ToListAsync();

            return forms;

        }


        public async Task<SourceTemplate> GetSourceTemplateById(int formId)
        {

            var formEntity = await _context.SourceTemplates
                                   .Include(f => f.TemplateSections)
                                       .ThenInclude(s => s.QuestionSectionMappings)
                                           .ThenInclude(qsm => qsm.Question)
                                               .ThenInclude(q => q.AnswerMasters)
                                                   .ThenInclude(am => am.AnswerOption)
                                   .FirstOrDefaultAsync(f => f.Id == formId && f.Active == true);




            return formEntity;
        }




        public async Task<bool> SoftDeleteFormAsync(int formId)
        {
            var form = await _context.SourceTemplates.FirstOrDefaultAsync(f => f.Id == formId && f.Active == true);

            if (form == null) return false;

            // Soft delete the form
            form.Active = false;
            form.DeletedOn = DateTime.Now;
            form.DeletedBy = 1;

            // Soft delete related sections
            var sections = await _context.TemplateSections
                .Where(s => s.FormId == formId && s.Active == true)
                .ToListAsync();

            foreach (var section in sections)
            {
                section.Active = false;
                section.DeletedOn = DateTime.Now;
                section.DeletedBy = 1;

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

    }
}
