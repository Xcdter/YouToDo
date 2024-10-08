using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouToDo.Data.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public char Title { get; set; }

        public char Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public List<Task> tasks { set; get; }
    }
}
