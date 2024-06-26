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
        public bool Finished { get; set; } = false;
        public DateTime DueDate { get; set; }

        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        public int? AsigneeID { get; set; } // ID of the user responsible for the task
        public UserEntity Asignee { get; set; }
    }
}
