using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Utils.Enums
{
    public class UserRole 
    {
        // User roles enumeration
        public enum UserRoleEnum
        {
            // Administrator - access to all CRUD operations
            Admin,

            // Moderator - access to create, read and update
            Moderator,

            // Regular User - access to create and read
            User
        }
        
        public static UserRole.UserRoleEnum GetUserRoleFromString(string role)
        {
            try
            {
                Console.WriteLine("Getting Role Enum from Role String.\n");
                return (UserRole.UserRoleEnum)Enum.Parse(typeof(UserRole.UserRoleEnum), role, true);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Getting Role Enum from Role String failed.\n");
                throw new Exception($"The role '{role}' is not valid.");
            }
        }
    }

}
