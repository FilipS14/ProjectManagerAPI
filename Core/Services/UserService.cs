using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataBase.Context;
using DataBase.Dtos;
using DataBase.Entities;
using DataBase.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Database.Repositories;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ProjectDbContext _context;
        private readonly object _userManager;

        public UserService(IConfiguration configuration, ProjectDbContext context, IUserRepository userRepository)
        {
            _configuration = configuration;
            _context = context;
            _userRepository = userRepository;
        }

        public Task<UserEntity> GetUserByIdAsync(int id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }

        public Task<UserEntity> GetUserByUsernameAsync(string username)
        {
            return _userRepository.GetUserByUsernameAsync(username);
        }

        public Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task AddUserAsync(UserEntity user)
        {
            return _userRepository.AddUserAsync(user);
        }

        public Task UpdateUserAsync(UserEntity user)
        {
            return _userRepository.UpdateUserAsync(user);
        }

        public Task DeleteUserAsync(int id)
        {
            return _userRepository.DeleteUserAsync(id);
        }
        private bool AllFiledIsCompleted(UserEntity user)
        {
            if (user.Username == null || user.Email == null || user.FullName == null || user.Password == null)
            {
                return false;
            }
            return true;
        }

        private bool FirstCharacterIsLetter(UserEntity user)
        {
            return char.IsLetter(user.Username[0]);
        }

        private bool UserExists(UserEntity user)
        {
            foreach(var u in _context.Users)
            {
                if (u.Username == user.Username)
                {
                    return true;
                }
                if (u.Email == user.Email)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsUserValid(UserEntity user)
        {
            if (FirstCharacterIsLetter(user) == false)
            {
                Console.WriteLine("! User invalid. Specified username is not valid.\n");
                return false;
            }
            if (IsEmailValid(user.Email) == false)
            {
                Console.WriteLine("! User invalid. Specified email is not valid.\n");
                return false;
            }
            if (!IsPassWordValid(user.Password))
            {
                Console.WriteLine("!PassWord invaild.");
                return false;
            }
            if (UserExists(user))
            {
                Console.WriteLine("! User invalid. This username / email is already taken.\n");
                return false;
            }

            return true;
        }

        private bool IsEmailValid(string email)
        {
            var emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private bool IsPassWordValid(string password)
        {
            return password.Length >= 8;
        }

        private async void RegisterValidUser(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Register(UserDto userDto)
        {
            try
            {
                var user = new UserEntity
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
                };

                if (IsUserValid(user)) 
                {
                    RegisterValidUser(user);
                }
                else
                    throw new Exception($"User could not be registered. Check internal console for more information about the error.");
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured while saving entity changes: {e.Message}", e);
            }

        }

        public async Task<string> Authenticate(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return null;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
