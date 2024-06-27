using System;
using System.Text.RegularExpressions;
using DataBase.Context;
using DataBase.Entities;
using DataBase.Repositories;
using Utils.Middleware.Exceptions;

namespace Core.Validation
{
    public class UserValidation
    {
        public static bool IsUserValidForRegister(UserEntity user, ProjectDbContext context)
        {
            Console.WriteLine($"Checking if the user exists");
            if (GetUserIdByUsernameOrEmail(user, context) != null)
            {
                throw new ValidationException("This username or email is already taken.", new List<string> { "Username or email already exists" });
                return false;
            }

            return IsUserInformationValid(user);
        }

        public static async Task<bool> IsUserValidForUpdate(UserEntity user, IUserRepository userRepository, ProjectDbContext context)
        {
            var existingUser = await userRepository.GetUserByIdAsync((int)user.Id);
            if (existingUser == null)
                throw new NotFoundException("No user with this ID exists.");

            if (IsUserInformationValid(user))
                return false;

            var userIds = GetUserIdByUsernameOrEmail(user, context);
            if (userIds != null)
            {
                foreach (var id in userIds)
                {
                    if (id != user.Id)
                        throw new ValidationException("Another user with the same username or email already exists.", new List<string> { "Duplicate username or email" });
                }
            }

            return true;
        }

        private static bool FirstCharacterIsLetter(UserEntity user)
        {
            return char.IsLetter(user.Username[0]);
        }

        private static List<int>? GetUserIdByUsernameOrEmail(UserEntity user, ProjectDbContext _context)
        {
            var matchingUserIds = _context.Users
                                        .Where(u => u.Username.Equals(user.Username) || u.Email.Equals(user.Email))
                                        .Select(u => (int)u.Id)
                                        .ToList();
                                        
            return matchingUserIds.Count == 0 ? null : matchingUserIds;
        }

        private static bool IsEmailValid(string email)
        {
            var emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private static bool IsPassWordValid(string password)
        {
            return password.Length >= 8;
        }

        private static bool IsUserInformationValid(UserEntity user)
        {
            if (!FirstCharacterIsLetter(user))
            {
                Console.WriteLine("! User invalid. Specified username is not valid.\n");
                throw new ValidationException("Specified username is not valid.", new List<string> { "Username must start with a letter" });
            }

            if (!IsEmailValid(user.Email))
            {
                Console.WriteLine("! User invalid. Specified email is not valid.\n");
                throw new ValidationException("Specified email is not valid.", new List<string> { "Invalid email format" });
            }

            if (!IsPassWordValid(user.Password))
            {
                Console.WriteLine("! Password invalid. The password must have minimum 8 characters.\n");
                throw new ValidationException("The password must have minimum 8 characters.", new List<string> { "Password too short" });
            }

            if (string.IsNullOrEmpty(user.FullName))
            {
                Console.WriteLine("! Full name invalid. Please enter your full name.\n");
                throw new ValidationException("Please enter your full name.", new List<string> { "Full name is required" });
            }
            return true;
        }

    }

}
