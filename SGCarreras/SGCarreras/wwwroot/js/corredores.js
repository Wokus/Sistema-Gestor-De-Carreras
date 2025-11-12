// Validación de contraseña en tiempo real
document.addEventListener('DOMContentLoaded', function () {
    const passwordInput = document.querySelector('input[type="password"]');
    const confirmPasswordInput = document.getElementById('ConfirmarContra');
    const passwordStrength = document.getElementById('password-strength');
    const passwordMatch = document.getElementById('password-match');

    // Crear elemento de fuerza de contraseña
    const strengthBar = document.createElement('div');
    strengthBar.className = 'password-strength';
    passwordInput.parentNode.appendChild(strengthBar);

    // Validar fuerza de contraseña
    passwordInput.addEventListener('input', function () {
        const password = this.value;
        let strength = 0;
        let message = '';

        if (password.length >= 8) strength++;
        if (password.match(/[a-z]/) && password.match(/[A-Z]/)) strength++;
        if (password.match(/\d/)) strength++;
        if (password.match(/[^a-zA-Z\d]/)) strength++;

        // Actualizar barra y mensaje
        strengthBar.className = 'password-strength';
        switch (strength) {
            case 0:
            case 1:
                strengthBar.classList.add('password-weak');
                message = 'Contraseña débil';
                break;
            case 2:
            case 3:
                strengthBar.classList.add('password-medium');
                message = 'Contraseña media';
                break;
            case 4:
                strengthBar.classList.add('password-strong');
                message = 'Contraseña fuerte';
                break;
        }

        passwordStrength.textContent = message;
        passwordStrength.className = strength === 4 ? 'text-success' : strength >= 2 ? 'text-warning' : 'text-danger';
    });

    // Validar coincidencia de contraseñas
    confirmPasswordInput.addEventListener('input', function () {
        const password = passwordInput.value;
        const confirmPassword = this.value;

        if (confirmPassword === '') {
            passwordMatch.textContent = '';
            return;
        }

        if (password === confirmPassword) {
            passwordMatch.textContent = 'Las contraseñas coinciden';
            passwordMatch.className = 'text-success password-match match-valid';
        } else {
            passwordMatch.textContent = 'Las contraseñas no coinciden';
            passwordMatch.className = 'text-danger password-match match-invalid';
        }
    });

    // Validación antes del envío
    document.querySelector('form').addEventListener('submit', function (e) {
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;
        const terminos = document.getElementById('terminos');

        if (password !== confirmPassword) {
            e.preventDefault();
            alert('Las contraseñas no coinciden. Por favor, verifique.');
            confirmPasswordInput.focus();
            return;
        }

        if (!terminos.checked) {
            e.preventDefault();
            alert('Debe aceptar los términos y condiciones para continuar.');
            return;
        }

        // Mostrar estado de carga
        const submitBtn = this.querySelector('button[type="submit"]');
        submitBtn.classList.add('btn-loading');
    });

    // Efectos visuales para inputs
    const inputs = document.querySelectorAll('.form-control, .form-select');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function () {
            if (this.value === '') {
                this.parentElement.classList.remove('focused');
            }
        });
    });
});