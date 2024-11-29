using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YouToDo.Filters;

namespace YouToDo.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен.")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов.")]
        public string Title { get; set; }

        [StringLength(10000, ErrorMessage = "Описание не должно превышать 10000 символов.")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "smallint")]
        public PriorityLevel Priority { get; set; }

        [StringLength(20, ErrorMessage = "Заголовок не должен превышать 20 символов.")]
        public string Tags { get; set; }

        public int? ProjectId { get; set; }

        public Project Project { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
