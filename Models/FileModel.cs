using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouToDo.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public TaskModel Task { get; set; }
    }
}

