﻿using Core.Services;
using DataBase.Context;
using DataBase.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Mappers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                await _userService.Register(userDto);
                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _userService.Authenticate(loginDto);
            
            if (token == null)
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }
            return Ok(new { token });
        }

        [HttpGet("getUserbyId")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getAllUsers")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetAll()
        { 
            
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("updateUser")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(UserDto userDto, int? userId = null)
        {
            try
            {
                if (userId < 0)
                    throw new Exception($"Please provide a valid ID");
                
                await _userService.UpdateUserAsync(UserMapper.ToEntity(userDto, userId));
                return Ok(new { message = "Update successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("deleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
