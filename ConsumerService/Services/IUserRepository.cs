using ConsumerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerService.Services
{
    public interface IUserRepository
    {
        Task<Users> Add(Users user);
        IEnumerable<Users> GetAll();
        Task<Users> Find(int userId);
        Task<Users> Update(Users user);
        Task<Users> Remove(int userId);
        Task<bool> Exist(int userId);
    }
}
