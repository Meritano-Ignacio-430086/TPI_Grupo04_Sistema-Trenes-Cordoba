const formRegister = document.getElementById("formRegister");
const inputEmail = document.getElementById("email");
const inputClave = document.getElementById("password");
const inputClave2 = document.getElementById("password2");
const mensajeError = document.getElementById("registerError");
const mensajeOk = document.getElementById("registerOk");
const btnRegister = document.getElementById("btnRegister");

function mostrarError(texto) {
    mensajeError.textContent = texto;
    mensajeError.hidden = !texto;
}

formRegister.addEventListener("submit", async (e) => {
    e.preventDefault();
    mostrarError("");
    mensajeOk.hidden = true;

    const email = inputEmail.value.trim();
    const clave = inputClave.value;

    if (!email || !clave || !inputClave2.value) {
        mostrarError("Completá todos los campos.");
        return;
    }
    if (!inputEmail.checkValidity()) {
        mostrarError("Ingresá un email válido.");
        return;
    }
    if (clave !== inputClave2.value) {
        mostrarError("Las contraseñas no coinciden.");
        return;
    }

    btnRegister.disabled = true;
    try {
        await apiPost("/api/Auth/registro", { email: email, password: clave });
        mensajeOk.textContent = "Cuenta creada. Te llevamos al login...";
        mensajeOk.hidden = false;
        setTimeout(() => location.href = "login.html", 1500);
    } catch (error) {
        console.error(error);
        // el backend responde 400 si el email ya existe o faltan datos
        mostrarError(error.status === 400 ? "Ya existe una cuenta con ese email." : error.message);
        btnRegister.disabled = false;
    }
});
