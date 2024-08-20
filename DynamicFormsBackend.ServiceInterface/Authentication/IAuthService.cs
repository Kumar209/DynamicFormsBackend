using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.ServiceInterface.Authentication
{
    public interface IAuthService
    {
        public Task<(bool success, string token)> AuthenticateUser(string username, string password);
    }
}
