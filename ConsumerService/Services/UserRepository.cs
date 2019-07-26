using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerService.Data;
using ConsumerService.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AutomationPlanContext _context;

        public UserRepository(AutomationPlanContext context)
        {
            _context = context;
        }

        public async Task<Users> Add(Users user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
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
