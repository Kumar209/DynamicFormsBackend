using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.ServiceInterface
{
    public interface IJwtService
    {
        public Task<string> GenerateToken(DynamicFormUser user);

        public Task<Dictionary<string, string>> ValidateJwtToken(string token);
    }
}
