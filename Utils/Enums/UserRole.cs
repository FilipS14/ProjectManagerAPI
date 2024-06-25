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
        /// <summary>
        /// User roles enumeration
        /// </summary>
        public enum UserRoleEnum
        {
            /// <summary>
            /// Administrator - access to all CRUD operations
            /// </summary>
            Admin,

            /// <summary>
            /// Moderator - access to create, read and update
            /// </summary>
            Moderator,

            /// <summary>
            /// Regular User - access to create and read
            /// </summary>
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
