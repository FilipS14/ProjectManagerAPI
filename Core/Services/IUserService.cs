using DataBase.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Entities;

namespace Core.Services
{
    public interface IUserService
    {        
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByIdAsync(int id);
        Task UpdateUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
        Task Register(UserDto userDto);
        Task<string> Authenticate(LoginDto loginDto);
    }
}
