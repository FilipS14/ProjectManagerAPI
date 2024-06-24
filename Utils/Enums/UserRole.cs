using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Enums
{
    public enum UserRole
    {
        Admin, // access to all CRUD operations
        Moderator, // access to create, read and update
        User // access to create and read
    }
}
