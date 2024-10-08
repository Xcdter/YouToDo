using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouToDo.Data.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        public char Title { get; set; }

        public char Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public short Priority { get; set; }

        public char Tags { get; set; }

        public List<File> files { set; get; }

        public virtual Project Project { set; get; }
    }
}
