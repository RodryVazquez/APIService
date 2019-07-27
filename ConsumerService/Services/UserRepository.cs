using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ConsumerService.Data;
using ConsumerService.Helpers;
using ConsumerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsumerService.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AutomationPlanContext _context;
        private readonly AppSettings _appSettings;

        public UserRepository(AutomationPlanContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<Users> Add(Users user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserAuthentication> Authenticate(string userName, string password)
        {
            var user = await _context.UserAuthentications.SingleOrDefaultAsync(x => x.Username == userName && x.Password == password);
            if (user == null)
                return null;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }

        public async Task<bool> Exist(int userId)
        {
            return await _context.Users.AnyAsync(p => p.UserId == userId);
        }

        public async Task<Users> Find(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<Users> Remove(int userId)
        {
            var user = await _context.Users.SingleAsync(m => m.UserId == userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> Update(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
