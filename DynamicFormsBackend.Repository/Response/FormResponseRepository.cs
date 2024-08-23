using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Repository.Response
{
    public class FormResponseRepository : IFormResponseRepository
    {

        private readonly ApplicationDbContext _context;

        public FormResponseRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<FormResponse> AddFormResponse(FormResponse response)
        {
            _context.FormResponses.Add(response);
            await _context.SaveChangesAsync();
            return response;
        }


        public async Task<IEnumerable<FormResponse>> GetAllResponsesByFormId(int formId)
        {
            var res = await _context.FormResponses
                .Where(fr => fr.FormId == formId && fr.Active == true)
                .ToListAsync();

            return res;
        }

        public async Task<FormResponse> GetResponseById(int responseId)
        {
            var res = await _context.FormResponses.FirstOrDefaultAsync(fr => fr.Id == responseId && fr.Active == true);
            return res;
        }


        public async  Task<bool> removeFormResponse(int responseId)
        {
            var response = await _context.FormResponses.FirstOrDefaultAsync(r => r.Id == responseId && r.Active == true);

            if(response == null) { return false; }

            response.Active = false;
            response.DeletedOn = DateTime.Now;
            response.DeletedBy = 1;

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
