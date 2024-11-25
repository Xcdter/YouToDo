using System.Collections.Generic;

namespace YouToDo.Models
{
    public class TaskProjectModel
    {
        public IEnumerable<TaskModel> Tasks { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public string FilteredPriority { get; set; }
    }
}
