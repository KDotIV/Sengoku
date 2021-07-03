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

        [HttpGet("GetUserEmail/{userId}", Name = "GetUserEmail")]
        public async Task<ActionResult> GetUserEmailAsync(string userId)
        {
            var userEmail = await _userRepository.GetUserEmail(userId);
            return Ok(new UserResponse(userEmail));
        }
        [HttpGet("GetUserById/{userId}", Name = "GetUserById")]
        public async Task<ActionResult> GetUserByIdAsync(string userId)
        {
            var result = await _userRepository.GetUserById(userId);
            return Ok(new UserResponse(result));
        }
        [HttpGet("GetUserByName/{userName}", Name = "GetUserByName")]
        public async Task<ActionResult> GetUserByNameAsync(string userName)
        {
            var result = await _userRepository.GetUserByName(userName);
            return Ok(new UserResponse(result));
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
            return Ok(response.User);
        }
    }
}
