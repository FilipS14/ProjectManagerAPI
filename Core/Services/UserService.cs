using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using DataBase.Context;
using DataBase.Dtos;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Database.Repositories;
using Utils.Enums;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ProjectDbContext _context;

        public UserService(IConfiguration configuration, ProjectDbContext context, IUserRepository userRepository)
        {
            _configuration = configuration;
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
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

        private async Task<bool> IsUserValidForUpdate(UserEntity user)
        {
            var existingUser = await GetUserByIdAsync((int)user.Id);
            if (existingUser == null)
                throw new Exception("No user with this ID exists.");

            if (IsUserInformationValid(user))
                return false;

            var userIds = GetUserIdByUsernameOrEmail(user);
            if (userIds != null)
            {
                foreach (var id in userIds)
                {
                    if (id != user.Id)
                        throw new Exception("Another user with the same username or email already exists.");
                }
            }

            return true;
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            try
            {
                if (await IsUserValidForUpdate(user) == null)
                    throw new Exception($"User could not be updated. Check internal console for more information about the error.");

                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).State = EntityState.Detached;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't update the user provided: {e}");
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var existingUser = await GetUserByIdAsync(id);
            if (existingUser == null)
                throw new InvalidOperationException("No user with this ID exists.");

            await _userRepository.DeleteUserAsync(id);
        }

        private bool FirstCharacterIsLetter(UserEntity user)
        {
            return char.IsLetter(user.Username[0]);
        }

        private List<int>? GetUserIdByUsernameOrEmail(UserEntity user)
        {
            var matchingUserIds = _context.Users
                                        .Where(u => u.Username.Equals(user.Username) || u.Email.Equals(user.Email))
                                        .Select(u => (int)u.Id)
                                        .ToList();
                                        
            return matchingUserIds.Count == 0 ? null : matchingUserIds;
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

        private bool IsUserInformationValid(UserEntity user)
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
                Console.WriteLine("! Password invaild. The password must have minimum 8 characters.\n");
                return false;
            }

            if (user.FullName == "")
            {
                Console.WriteLine("! Full name invalid. Please enter your full name.\n");
                return false;
            }
            return true;
        }

        private bool IsUserValidForRegister(UserEntity user)
        {
            Console.WriteLine($"Checking if the user exists");
            if (GetUserIdByUsernameOrEmail(user) != null)
            {
                Console.WriteLine("! User invalid. This username / email is already taken.\n");
                return false;
            }

            return IsUserInformationValid(user);
        }

        public async Task Register(UserDto userDto)
        {
            try
            {
                Console.WriteLine("Entered Register in UserService\n");
                var user = new UserEntity
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    Role = UserRole.GetUserRoleFromString(userDto.Role)
                };
                if (IsUserValidForRegister(user))
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
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
