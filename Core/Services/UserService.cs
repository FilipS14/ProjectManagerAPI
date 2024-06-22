using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Entities;
using DataBase.Repositories;
using Project.Database.Repositories;




namespace Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
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
    }
}
