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
        public Task<(bool success, int formid)> AddSourceTemplate(SourceTemplateDto sourceTemplateDetails);

        public Task<IEnumerable<SourceTemplate>> Getforms();


        public Task<FetchFormDto> GetFormById(int formId);


        public Task<bool> RemoveFormById(int formId);
    }
}
