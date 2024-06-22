using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public int? UserId { get; set; }
        public UserEntity User { get; set; }

    }
}
