using DynamicFormsBackend.RepositoryInterface.Authentication;
using DynamicFormsBackend.ServiceInterface;
using DynamicFormsBackend.ServiceInterface.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Service.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        public AuthService(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }


    

        public async Task<(bool success, string token)> AuthenticateUser(string username, string password)
        {
            var user = await _authRepository.GetUserByEmailPassword(username, password);

            if (user == null) 
            {
                return (false, null);
            }

            var token = await _jwtService.GenerateToken(user);

            return (true, token);

        } 


    }
}
