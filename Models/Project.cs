using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using YouToDo.Filters;

namespace YouToDo.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен.")]
        [StringLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов.")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DueDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
