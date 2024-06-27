using DataBase.Entities;
using DataBase.Repositories;

namespace DataBase.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByIdAsync(int id);
        Task<UserEntity> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task AddUserAsync(UserEntity user);
        Task UpdateUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
    }
}