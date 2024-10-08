using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouToDo.Data.Models
{
    public class File
    {
        public int UserId { get; set; }

        public char FileName { get; set; }

        public char Type { get; set; }

        public char Password { get; set; }
    }
}
