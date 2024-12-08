using System.ComponentModel.DataAnnotations;

namespace YouToDo.Attributes
{
    public class RequireIfOldPasswordProvidedAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;

        public RequireIfOldPasswordProvidedAttribute(string dependentProperty)
        {
            _dependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentValue = validationContext.ObjectType
                .GetProperty(_dependentProperty)
                ?.GetValue(validationContext.ObjectInstance) as string;

            // Если зависимое поле (старый пароль) заполнено
            if (!string.IsNullOrEmpty(dependentValue))
            {
                // Проверяем текущее поле (новый пароль или подтверждение)
                if (string.IsNullOrEmpty(value as string))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} обязательно для заполнения.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
