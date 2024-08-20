using DynamicFormsBackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.RepositoryInterface.Authentication
{
    public interface IAuthRepository
    {
        public Task<DynamicFormUser> GetUserByEmailPassword(string email, string password);
    }
}
