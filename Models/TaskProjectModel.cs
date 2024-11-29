using System.Collections.Generic;

namespace YouToDo.Models
{
    public class TaskProjectModel
    {
        public IEnumerable<TaskModel> Tasks { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public string FilteredPriority { get; set; }
        public short? FilteredPriorityValue { get; set; }

        public string ActiveTag { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int? ActiveProjectId { get; set; }
    }
}
