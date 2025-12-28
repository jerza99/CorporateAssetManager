// Lógica para mostrar/ocultar contraseña
const togglePassword = document.querySelector('#togglePasswordIcon');
const password = document.querySelector('#passwordInput');

if (togglePassword && password) {
    togglePassword.addEventListener('click', function () {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        this.classList.toggle('bi-eye');
        this.classList.toggle('bi-eye-slash');
    });
}

// Lógica para mostrar/ocultar confirmación de contraseña (Register)
const toggleConfirmPassword = document.querySelector('#toggleConfirmPasswordIcon');
const confirmPassword = document.querySelector('#confirmPasswordInput');

if (toggleConfirmPassword && confirmPassword) {
    toggleConfirmPassword.addEventListener('click', function () {
        const type = confirmPassword.getAttribute('type') === 'password' ? 'text' : 'password';
        confirmPassword.setAttribute('type', type);
        this.classList.toggle('bi-eye');
        this.classList.toggle('bi-eye-slash');
    });
}