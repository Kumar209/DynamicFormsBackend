using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.RepositoryInterface.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Repository.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DynamicFormUser> GetUserByEmailPassword(string email, string password)
        {
            var user = await _context.DynamicFormUsers
                       .FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.Active == true);

            return user;
        }
    }
}
