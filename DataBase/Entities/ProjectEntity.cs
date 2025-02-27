﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public int AsigneeID { get; set; } // ID of an user responsible for the project / Project Manager
        public UserEntity Asignee { get; set; }
        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
