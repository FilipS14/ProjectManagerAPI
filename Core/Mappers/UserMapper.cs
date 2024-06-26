using DataBase.Dtos;
using DataBase.Entities;
using Utils.Enums;

namespace Core.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(UserEntity user)
        {
            var userDto =  new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
            
            if (user.Id.HasValue)
            {
                userDto.Id = user.Id;
            }

            return userDto;
        }

        public static UserEntity ToEntity(UserDto userDto, int? userId = null)
        {
            var userEntity = new UserEntity
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                FullName = userDto.FullName,
                Role = UserRole.GetUserRoleFromString(userDto.Role)
            };

            if (userId.HasValue)
            {
                userEntity.Id = userId.Value;
            }

            return userEntity;
        }
    }
}
