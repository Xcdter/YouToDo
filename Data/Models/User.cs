using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouToDo.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public char Name { get; set; }

        public char Email { get; set; }

        public char Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Task> tasks { set; get; }

        public List<Project> projecrs { set; get; }
    }
}
