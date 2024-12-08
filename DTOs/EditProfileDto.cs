using System.ComponentModel.DataAnnotations;
using YouToDo.Attributes;

namespace YouToDo.DTOs
{
    public class EditProfileDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Имя обязательно для заполнения.")]
        [StringLength(20, ErrorMessage = "Имя не должно превышать 20 символов.")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Пароль должен быть длиной от {2} до {1} символов.", MinimumLength = 4)]
        [RequireIfOldPasswordProvided("OldPassword", ErrorMessage = "Новый пароль обязателен, если введен старый пароль.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        [RequireIfOldPasswordProvided("OldPassword", ErrorMessage = "Подтверждение пароля обязательно, если введен старый пароль.")]
        public string ConfirmPassword { get; set; }
    }
}
