using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using Sengoku.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult> GetUsersAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var users = await _userRepository.GetUsers(limit, page, sort);
            return Ok(users);
        }

        [HttpGet("GetUser/email/{userId}")]
        public async Task<ActionResult> GetUserEmailAsync(string userId)
        {
            var userEmail = await _userRepository.GetUserEmail(userId);
            return Ok(userEmail);
        }
        [HttpGet("GetUser/id/{userId}", Name = "GetUserId")]
        public async Task<ActionResult> GetUserByIdAsync(string userId)
        {
            var result = await _userRepository.GetUserById(userId);
            return Ok(result);
        }
        [HttpGet("GetUser/username/{userName}", Name = "GetUserName")]
        public async Task<ActionResult> GetUserByNameAsync(string userName)
        {
            var result = await _userRepository.GetUserByName(userName);
            return Ok(result);
        }
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] User user)
        {
            var response = await _userRepository.AddUserAsync(user.Name, user.Email, user.Password);
            if(!response.Success)
            {
                return BadRequest(new { error = response.ErrorMessage });
            }
            return Ok(response);
        }
        [HttpDelete("DeleteUser/{email}")]
        public async Task<ActionResult> DeleteUserAsync(string email)
        {
            var result = await _userRepository.DeleteUser(email);
            return Ok(result);
        }
        [HttpPut("UpdateUser/email/{userId}")]
        public async Task<ActionResult> UpdateUserEmailAsync([FromBody] User user, string userId)
        {
            var userToUpdate = await _userRepository.GetUserById(userId);
            var result = await _userRepository.UpdateUserEmail(userToUpdate.Email, user.Email);
            return Ok(result);
        }
    }
}