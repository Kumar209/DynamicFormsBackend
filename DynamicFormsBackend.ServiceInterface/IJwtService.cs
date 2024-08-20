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

        public Task<int> ValidateJwtToken(string token);
    }
}
