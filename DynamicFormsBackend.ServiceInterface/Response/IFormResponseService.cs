using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.ServiceInterface.Response
{
    public interface IFormResponseService
    {
        public Task<bool> AddFormResponse(FormResponseDto responseDto);

        public Task<IEnumerable<FormResponseDto>> GetAllResponse(int formId);

        public Task<FormResponseDto> GetResponse(int responseId);
    }
}
