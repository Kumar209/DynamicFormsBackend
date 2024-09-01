using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.ServiceInterface.FormCreation
{
    public interface IFormService
    {
        public Task<(bool success, int formid)> AddSourceTemplate(SourceTemplateDto sourceTemplateDetails, int userId);

        public Task<IEnumerable<SourceTemplate>> Getforms(int userId);


        public Task<FetchFormDto> GetFormById(int formId, int userId);


        public Task<bool> RemoveFormById(int formId, int userId);



        public Task<bool> UpdateSourceTemplate(int formId, SourceTemplateDto sourceTemplateDetails, int userId);
    }
}
