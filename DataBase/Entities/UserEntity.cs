using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Utils.Enums;

namespace DataBase.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        
        public ICollection<ProjectEntity> Projects { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }

    }
}
