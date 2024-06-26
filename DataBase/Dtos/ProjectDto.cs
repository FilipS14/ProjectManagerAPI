using DataBase.Dtos;
using System.Text.Json.Serialization;

namespace DataBase.Dtos
{
    public class ProjectDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public int AsigneeID { get; set; } // ID of the user responsible for the project / Project Manager

        [JsonIgnore]
        public List<TaskDto>? Tasks { get; set; }
    }
}
