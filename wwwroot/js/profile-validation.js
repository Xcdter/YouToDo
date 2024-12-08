document.addEventListener("DOMContentLoaded", function () {
    const oldPassword = document.getElementById("OldPassword");
    const newPassword = document.getElementById("NewPassword");
    const confirmPassword = document.getElementById("ConfirmPassword");
    const nameField = document.getElementById("Name");
    const saveButton = document.querySelector("button[type='submit']");

    if (oldPassword) {
        oldPassword.addEventListener("input", function () {
            const isOldPasswordFilled = oldPassword.value.trim() !== "";

            if (newPassword) {
                newPassword.disabled = !isOldPasswordFilled;
            }

            if (confirmPassword) {
                confirmPassword.disabled = !isOldPasswordFilled;
            }
        });
    }

    if (nameField && saveButton) {
        const initialName = nameField.value.trim();

        nameField.addEventListener("input", function () {
            const currentName = nameField.value.trim();
            saveButton.disabled = currentName === initialName;
        });

        // Проверка при загрузке страницы
        saveButton.disabled = nameField.value.trim() === initialName;
    }
});

