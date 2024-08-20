using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.RepositoryInterface.Response
{
    public interface IFormResponseRepository
    {
        public Task<FormResponse> AddFormResponse(FormResponse response);

        public Task<IEnumerable<FormResponse>> GetAllResponsesByFormId(int formId);

        public Task<FormResponse> GetResponseById(int responseId);
    }
}
