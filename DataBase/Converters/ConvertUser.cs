using DataBase.Dtos;
using DataBase.Entities;
using Utils.Enums;

namespace DataBase.Converters
{
    public class ConvertUser
    {
        public static UserEntity ConvertUserFromDtoToEntity(UserDto user, int userId = -1)
        {
            // search if the user exists in the database and return the UserEntity of it

            // else return UserEntity with all the information from the Dto

            if (userId == -1) 
                return new UserEntity
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    FullName = user.FullName,
                    Role = UserRole.GetUserRoleFromString(user.Role)
                };
            else
                return new UserEntity
                {
                    Id = userId,
                    Username = user.Username,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    FullName = user.FullName,
                    Role = UserRole.GetUserRoleFromString(user.Role)
                };
        }

    }

}
