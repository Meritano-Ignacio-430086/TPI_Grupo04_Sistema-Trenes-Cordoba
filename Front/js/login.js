const formLogin = document.getElementById("formLogin");
const inputUsuario = document.getElementById("username");
const inputClave = document.getElementById("password");
const checkRecordar = formLogin.querySelector('input[name="remember"]');
const mensajeError = document.getElementById("loginError");
const btnLogin = document.getElementById("btnLogin");

// Si ya hay una sesión abierta, se salta el login
if (obtenerToken()) {
    location.replace("dashboard.html");
}

function mostrarError(texto) {
    mensajeError.textContent = texto;
    mensajeError.hidden = !texto;
}

formLogin.addEventListener("submit", async (e) => {
    e.preventDefault();
    mostrarError("");

    const email = inputUsuario.value.trim();
    const clave = inputClave.value;
    if (!email || !clave) {
        mostrarError("Ingresá tu usuario y tu contraseña.");
        return;
    }

    btnLogin.disabled = true;
    try {
        const respuesta = await apiPost("/api/Auth/login", { userEmail: email, password: clave });
        if (!respuesta?.token) throw new Error("La API no devolvió un token.");
        guardarSesion(respuesta.token, email, checkRecordar.checked);
        location.href = "dashboard.html";
    } catch (error) {
        console.error(error);
        mostrarError(error.status === 401 ? "Usuario o contraseña incorrectos." : error.message);
    } finally {
        btnLogin.disabled = false;
    }
});
