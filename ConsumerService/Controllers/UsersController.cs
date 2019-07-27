using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerService.Models;
using ConsumerService.Models.UserViewModel;
using ConsumerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        
        public UsersController(IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthentication user)
        {
            var userAuth = await _userRepository.Authenticate(user.Username, user.Password);
            if (userAuth == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(userAuth);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }
        
        private async Task<bool> UserExist(int userId)
        {
            var exist = await _userRepository.Exist(userId);
            return exist;    
        }
        
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var user = await _userRepository.Find(userId);
            if (user == null) { return BadRequest(); } 
            return Ok(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserViewModel user)
        {
            var createdUser = await _userRepository.Add(user.ToEntity());
            return CreatedAtAction("GetUser", new { id = createdUser.UserId}, user);
        }
        
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser([FromRoute] int userId, [FromBody] UserViewModel user)
        {
            try
            {
                var createdUser =  await _userRepository.Update(user.ToEntity());
                return Ok(createdUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        
        [HttpDelete("userId")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            var user = await _userRepository.Remove(userId);
            return Ok(user);
        }
    }
}